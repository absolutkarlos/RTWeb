var home = (function () {
	return {
		init: function () {
			this.GetEvent().Select2Event();
			this.GetEvent().WindowScrollEvent();
			this.GetEvent().ButtomScrollDownEvent();
			this.GetEvent().RefreshPanelOrders();
			this.GetEvent().ViewInfoOrder();
			this.GetEvent().AddNewOrder();
			this.GetEvent().ExistingOrder();
			this.GetEvent().ClientsEvent();
			this.LoadMetaData();
			this.InitializeOrderStatusBar();
			base.ApplyNiceScroll("html");
			base.ApplyNiceScroll("#contentPanelOrders");
			$("#refreshInfoOrderPanel").hide();
			$('[rel="tooltip"]').tooltip();
			$(".js-example-basic-multiple").select2();
			preFactibility.init();
			inspection.init();
			instalation.init();
			$('nav').css({ "background-color": "rgba(0, 0, 0, 0.65)" });
		},

		LoadDataDropDowns: function (data) {
			base.LoadDropDownList("#contactCountry", data.Countries.Data);
			base.LoadDropDownList("#country", data.Countries.Data);
			base.LoadDropDownList("#position", data.Position.Data);
			base.LoadDropDownList("#entitytype", data.EntityType.Data);
			base.LoadDropDownList("#servicetype", data.ServiceType.Data);
			base.LoadDropDownList("#clienttype", data.ClientType.Data);
			base.LoadDropDownList("#accesstype", data.AccessType.Data);
			base.LoadDropDownList("#celdas", data.RadioBase.Data);
		},

		LoadInfoOrderPanel: function (id, clearFileInput) {
			$("#panelInfo").show();
			$("#panelClient").hide();
			$("#refreshInfoOrderPanel").show();
			$("#info").hide();
			$("#next").hide();
			$('.wizard-card').bootstrapWizard('show', 0);
			if ($("#input-700NOC") && clearFileInput)
				$("#input-700NOC").fileinput("clear");
			home.GetEvent().IconRefreshAnimated("#refreshInfoOrder");
			$.when(baseHome.LoadInfoOrderPanel(id)).then(function (panel) {
				$("#next").show();
				home.GetEvent().IconRefreshAnimatedStop("#refreshInfoOrder");
				$("#info").remove();
				$("#refreshInfoOrderPanel").hide();
				$("#contentPanelInfo").prepend(panel);
				home.InitializeOrderStatusBar();
				site.GetEvent().CreteOrderButtomEvent();
				home.GetEvent().AddNewOrder();
				$('[data-toggle="popover"]').popover();
				$(".js-example-basic-multiple").select2();
				preFactibility.ValidateShowButtons();
				inspection.ValidateShowButtons();
				instalation.ValidateShowButtons();
				$("#input-700NOC").fileinput('clear');
				base.ApplyNiceScroll("scrollContactInfo");
			});
		},

		UpdateOrderPanel: function () {
			$.when(baseHome.UpdatePanelOrder()).then(function (orders) {
				$("#contentPanelOrders").remove();
				$("#panelordenes").append(orders);
				home.GetEvent().IconRefreshAnimatedStop("#refreshOrders");
				base.ApplyNiceScroll("#contentPanelOrders");
				home.GetEvent().ViewInfoOrder();
			});
		},

		LoadMetaData: function () {
			$.when(baseHome.GetAllMetaData()).then(function (result) {
				if (result) {
					base.RemoveLocalMetaData();
					localStorage.setItem("MetaData", JSON.stringify(result));
					home.LoadDataDropDowns(result);
					home.GetEvent().DropDownCountryChange();
					home.GetEvent().DropDownClientTypeChange();
					base.LoadRadioBase();
				} else {
					base.ValidateHasError(result, function () {
						alert("Error Cargando la MetaData");
					});
				}
			});
		},

		InitializeOrderStatusBar: function () {
			$('#rootwizard').bootstrapWizard({ 'tabClass': 'bwizard-steps' });
		},

		GetExistingClient: function (clientId) {
			if (clientId !== "") {
				$.when(baseHome.GetExistingClient(clientId)).then(function (metaData) {
					if (metaData && (metaData.Data)) {
						$("#labelClientType").text("(" + metaData.Data.ClientType.Name + ")");
						$("#labelUbicacion").text(metaData.Data.City.Name + ", " + metaData.Data.State.Name + ", " + metaData.Data.Country.Name);
						$("#labelFullName").text(metaData.Data.LegalName);
						$("#labelBusinessName").text(metaData.Data.BusinessName);
						$("#labelRuc").text(metaData.Data.Ruc);
						$("#labelDetailedAdress").text(metaData.Data.AddressRef);
						$("#refreshExistingClient").css({ "display": "none" });
						$(".readOnly").show();
						$("#wizardClient").find('.btn-next').show();
						contact.ClearContacts();
						contact.LoadContactList(metaData.Data.ListEntityContact);
					}
				});
			} else {
				$("#refreshExistingClient").css({ "display": "none" });
				$("#wizardClient").find('.btn-next').hide();
			}
		},

		GetAllClients: function () {
			$.when(baseHome.GetAllClients()).then(function (metaData) {
				if (metaData && (metaData.Data) && (metaData.Data.length > 0)) {
					$validator.resetForm();
					$("#refreshExistingClient").css({ "display": "none" });
					$(".existingClient").show();
					base.ClearDropDownList("#clients");
					$.each(metaData.Data, function () {
						$("#clients").append($("<option data-ruc='" + this.Ruc + "'/>").val(this.Id).text(this.BusinessName));
					});
				} else {
					$('#tabContact').find('a').prop('disabled', true);
					$('#tabLocalization').find('a').prop('disabled', true);
					$("#wizardClient").find('.btn-next').hide();
					$("#clientNotFound").show();
					$("#refreshExistingClient").hide();
				}
			});
		},

		GetEvent: function () {
			return {
				ClientsEvent: function () {
					$("#clients").change(function () {
						$(".readOnly").hide();
						$("#refreshExistingClient").css({ "display": "table" });
						client.LoadClientId($(this).val());
						home.GetExistingClient($(this).val());
						$("#wizardClientForm").validate().element("#clients");
					});
				},

				AddNewOrder: function () {
					$('#newOrder').click(function (event) {
						$validator.resetForm();
						$('#wizardClient').bootstrapWizard('show', 0);
						$('#tabContact').find('a').prop('disabled', false);
						$('#tabLocalization').find('a').prop('disabled', false);
						$("#wizardClient").find('.btn-next').show();
						$("#clientNotFound").hide();
						$("#panelInfo").hide();
						$("#panelClient").show();
						$(".edit").show();
						$(".readOnly").hide();
						$(".existingClient").hide();
						$("#clienttype").val("1").trigger("change");
					});
				},

				ExistingOrder: function () {
					$('#existingOrder').click(function (event) {
						$("#panelInfo").hide();
						$("#panelClient").show();
						$('#wizardClient').bootstrapWizard('show', 0);
						$(".edit").hide();
						$(".readOnly").hide();
						$("#refreshExistingClient").css({ "display": "table" });
						$("#wizardClient").find('.btn-next').hide();
						home.GetAllClients();
					});
				},

				AddNewOrderExistingClient: function () {
					$('#newOrderExistingClient').click(function (event) {
						$validator.resetForm();
						$('#wizardClient').bootstrapWizard('show', 0);
						$("#panelInfo").hide();
						$("#panelClient").show();
					});
				},

				ViewInfoOrder: function () {
					$('.order').click(function (event) {
						event.preventDefault();
						home.LoadInfoOrderPanel($(this).data("orderid"), true);
					});
				},

				RefreshPanelOrders: function () {
					$('#refreshOrders').click(function () {
						home.UpdateOrderPanel();
					});
				},

				IconRefreshAnimated: function (selector) {
					var animateClass = "icon-refresh-animate";
					$(selector).addClass(animateClass);
				},

				IconRefreshAnimatedStop: function (selector) {
					var animateClass = "icon-refresh-animate";
					$(selector).removeClass(animateClass);
				},

				DropDownClientTypeChange: function () {
					$('#clienttype').change(function () {
						$("#wizardClientForm").validate().element("#clienttype");
					});
				},

				DropDownZoneChange: function () {
					$('#zone').change(function () {
						$("#wizardClientForm").validate().element("#zone");
					});
				},

				DropDownCityChange: function () {
					home.GetEvent().DropDownZoneChange();
					$('#city').change(function () {
						base.ClearDropDownList("#zone");
						$.when(baseHome.GetZonesByCity($('#city').val())).then(function (metaData) {
							if (metaData.Data && metaData.Data.length > 0) {
								base.LoadDropDownList("#zone", metaData.Data);
							} else {
								base.ValidateHasError(metaData, function () {
									alert("Error Cargando las zonas");
								});
							}
						});
						$("#wizardClientForm").validate().element("#city");
						$("#wizardClientForm").validate().element("#zone");
					});
				},

				DropDownStateChange: function () {
					home.GetEvent().DropDownCityChange();
					$('#state').change(function () {
						base.ClearDropDownList("#city");
						base.ClearDropDownList("#zone");
						$.when(baseHome.GetCitiesByState($('#state').val())).then(function (metaData) {
							if (metaData.Data && metaData.Data.length > 0) {
								base.LoadDropDownList("#city", metaData.Data);
							} else {
								base.ValidateHasError(metaData, function () {
									alert("Error Cargando las ciudades");
								});
							}
						});
						$("#wizardClientForm").validate().element("#state");
						$("#wizardClientForm").validate().element("#city");
						$("#wizardClientForm").validate().element("#zone");
					});
				},

				DropDownCountryChange: function () {
					home.GetEvent().DropDownStateChange();
					$('#country').change(function () {
						base.ClearDropDownList("#state");
						base.ClearDropDownList("#city");
						base.ClearDropDownList("#zone");
						$.when(baseHome.GetStatesByCountry($('#country').val())).then(function (metaData) {
							if (metaData.Data && metaData.Data.length > 0) {
								base.LoadDropDownList("#state", metaData.Data);
							} else {
								base.ValidateHasError(metaData, function () {
									alert("Error Cargando los estados");
								});
							}
						});
						$("#wizardClientForm").validate().element("#country");
						$("#wizardClientForm").validate().element("#state");
						$("#wizardClientForm").validate().element("#city");
						$("#wizardClientForm").validate().element("#zone");
					});
				},

				AnimateScrollContentPage: function (control) {
					$('#' + control).animatescroll({
						scrollSpeed: 500,
						//easing: 'easeOutBounce',
						onScrollStart: function () {
							//$('nav').css({ "background-color": "rgba(0, 0, 0, 0.65)" });
						},
						onScrollEnd: function () {

						}
					});
				},

				WindowScrollEvent: function () {
					var lastScrollTop = 0;
					$(window).scroll(function (event) {
						var st = $(this).scrollTop();
						if (st > lastScrollTop) {
							//$('nav').css({ "background-color": "rgba(0, 0, 0, 0.65)" });
						} else {
							//if ($(window).scrollTop() <= ($("header").height() - 75))
								//$('nav').css({ "background-color": "rgba(0, 0, 0, 0)" });
						}
						lastScrollTop = st;
					});
				},

				WindowMouseWheelEvent: function () {
					$(window).bind('mousewheel', function (event) {
						if ((event.originalEvent.wheelDelta >= 0) && ($(window).scrollTop() <= ($("header").height() - 75))) {
							$('header').animatescroll({
								onScrollStart: function () {
									//if ($(window).scrollTop() <= ($("header").height() - 75))
									//	$('nav').css({ "background-color": "rgba(0, 0, 0, 0)" });
								},
								onScrollEnd: function () {

								}
							});
						}
						else {
							//if ($(window).scrollTop() <= ($("header").height() - 75)) {
							//	home.GetEvent().AnimateScrollContentPage("page");
							//}
						}
					});
				},

				Select2Event: function () {
					$(".select2").select2({
						language: "es"
					});
				},

				ButtomScrollDownEvent: function () {
					$(".btn-scroll-down").click(function () {
						home.GetEvent().AnimateScrollContentPage("page");
					});
				}
			}
		}
	}
}());

$(function () {
	home.init();
});