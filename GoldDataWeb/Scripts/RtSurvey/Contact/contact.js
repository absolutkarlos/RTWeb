var contact = (function () {

	return {
		init: function () {
			this.GetEvent().DropDownContactCountryChange();
			this.GetEvent().AddContactEvent();
			this.GetEvent().DropDownPositionChange();
			$('.bfh-selectbox').on("change.bfhselectbox", function () {
				$('#selectContryFlag option[value="' + $(this).val() + '"]').prop("selected", true);
				$("#selectContryFlag").change();
			});
		},

		ContactsIsValid: function () {
			return $(".contacts").length > 0;
		},

		ClearContacts: function () {
			$(".contacts").remove();
			$(".collapse").remove();
			$("#contactListEmpty").show();
		},

		LoadContactList: function (contacts) {
			if (contacts != null && contacts.length > 0) {
				$.each(contacts, function (index, item) {
					$("#listContact").append("<a href='#" + index + "' class='list-group-item contacts' style='padding: 0; height: 42px;' data-toggle='collapse' aria-expanded='false'>" +
														'<span class="pull-left" style="height: 100%; padding: 10px;">' + item.Name.toUpperCase() + ' <small style="color: #999999;"> (' + item.Position.Name + ') </small></span>' +
														'<span style="display:none;" data-idremove="' + index + '" class="contact-buttom-remove pull-right"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></span></a>' +
														'<div class="collapse" id="' + index + '" style="position: relative; padding: 10px 15px; margin-bottom: -1px; background-color: #fff; border: 1px solid #ddd;">' +
														'<div class="card card-block" style="border: 1px solid #ddd; padding: 10px; border-radius: 5px;">' +
														'<b>Telefono:</b> ' + item.ListEntityChannels[0].Channel + '<br/> <b>Email:</b> ' + (item.ListEntityChannels[1] != null ? item.ListEntityChannels[1].Channel : "N/A") + '<br/> <b>Dirección:</b> ' + item.Zone.Name + ', ' +
														item.State.Name + ', ' + item.Country.Name + '</div></div>');
				});
				$("#contactListEmpty").hide();
			}
		},

		GetContactList: function () {
			var contacts = [];
			if (contact.ContactsIsValid()) {
				$.each($(".contacts"), function (index, item) {
					contacts.push($(item).data("entitycontact"));
				});
			}
			return JSON.stringify(contacts);
		},

		ContactCreate: function (entityContact) {
			return baseContact.ContactsCreate(entityContact).success(function (response) {
				return base.ValidateHasError(response, function () {
					alert("error creando los contactos");
				});
			});
		},

		GetEntityChannels: function () {
			var entityChannels = [{
					"Channel": $("#phone").val(),
					"EntityType.Id": 2,
					"EntityChannelType.Id": 3
				}, {
					"Channel": $("#email").val(),
					"EntityType.Id": 2,
					"EntityChannelType.Id": 1
				}];
			var listEntityChannels = [];
			if (entityChannels.length > 0) {
				$.each(entityChannels, function (index, item) {
					listEntityChannels.push(item);
				});
			}
			return listEntityChannels;
		},

		GenerateContact: function () {
			var entityContact = {
				"EntityContact.Name": $("#contactfullname").val(),
				"EntityContact.IdEntity": client.GetClientId(),
				"EntityContact.Position.Id": $("#position").val(),
				"EntityContact.EntityType.Id": 2,
				"EntityContact.Country.Id": $("#contactCountry").val(),
				"EntityContact.State.Id": $("#contactState").val(),
				"EntityContact.City.Id": $("#contactCity").val(),
				"EntityContact.Zone.Id": $("#contactZone").val(),
				"EntityContact.ListEntityChannels": this.GetEntityChannels()
			}

			return JSON.stringify(entityContact);
		},

		ResetForm: function () {
			base.ResetDropDownList("#contactCountry");
			base.ResetDropDownList("#position");
			base.ClearDropDownList("#contactState");
			base.ClearDropDownList("#contactCity");
			base.ClearDropDownList("#contactZone");
			$("#contactfullname").val("");
			$("#phone").val("");
			$("#email").val("");
		},

		GetEvent: function () {
			return {
				AddContactEvent: function () {
					$('#addContact').click(function () {

						if ($("#wizardClientForm").valid()) {
							var entityContact = contact.GenerateContact();
							var response = contact.ContactCreate(entityContact);

							if (response.responseJSON.ErrorMessage == null && response.responseJSON.Data != null) {
								$("#contactListEmpty").hide();
								var index = $(".contacts").length + 1;

								$("#listContact").append("<a href='#" + index + "' class='list-group-item contacts' style='padding: 0; height: 42px;' data-toggle='collapse' aria-expanded='false'>" +
									'<span class="pull-left" style="height: 100%; padding: 10px;">' + $("#contactfullname").val().toUpperCase() + ' <small style="color: #999999;"> (' + $("#position option[value='" + $("#position").val() + "']").text() + ') </small></span>' +
									'<span style="display:none;" data-idremove="' + index + '" class="contact-buttom-remove pull-right"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></span></a>' +
									'<div class="collapse" id="' + index + '" style="position: relative; padding: 10px 15px; margin-bottom: -1px; background-color: #fff; border: 1px solid #ddd;">' +
									'<div class="card card-block" style="border: 1px solid #ddd; padding: 10px; border-radius: 5px;">' +
									'<b>Telefono:</b> ' + $("#phone").val() + '<br/> <b>Email:</b> ' + $("#email").val() + '<br/> <b>Dirección:</b> ' + $("#contactZone option[value='" + $("#contactZone").val() + "']").text() + ', ' +
									$("#contactState option[value='" + $("#contactState").val() + "']").text() + ', ' + $("#contactCountry option[value='" + $("#contactCountry").val() + "']").text() + '</div></div>');

								//contact.GetEvent().ContactButtomRemoveEvent();

								//validator.resetForm();
								contact.ResetForm();
							}
						}
					});
				},

				ContactButtomRemoveEvent: function () {
					$(".contact-buttom-remove").unbind("click");
					$(".contact-buttom-remove").click(function () {
						$("#" + $(this).data("idremove")).remove();
						$(this).parent().fadeOut(300, function () {
							$(this).remove();
							if ($(".contacts").length === 0) {
								$("#contactListEmpty").fadeIn(300, function () {
									$(this).show();
								});
							}
						});
					});
				},

				DropDownPositionChange: function () {
					$('#position').change(function () {
						$("#wizardClientForm").validate().element("#position");
					});
				},

				DropDownZoneChange: function () {
					$('#contactZone').change(function () {
						$("#wizardClientForm").validate().element("#contactZone");
					});
				},

				DropDownContactCityChange: function () {
					contact.GetEvent().DropDownZoneChange();
					$('#contactCity').change(function () {
						base.ClearDropDownList("#contactZone");
						$.when(baseHome.GetZonesByCity($('#contactCity').val())).then(function (metaData) {
							if (metaData.Data && metaData.Data.length > 0) {
								base.LoadDropDownList("#contactZone", metaData.Data);
							} else {
								base.ValidateHasError(metaData, function () {
									alert("Error Cargando las zonas");
								});
							}
						});
						$("#wizardClientForm").validate().element("#contactCity");
						$("#wizardClientForm").validate().element("#contactZone");
					});
				},

				DropDownContactStateChange: function () {
					contact.GetEvent().DropDownContactCityChange();
					$('#contactState').change(function () {
						base.ClearDropDownList("#contactCity");
						base.ClearDropDownList("#contactZone");
						$.when(baseHome.GetCitiesByState($('#contactState').val())).then(function (metaData) {
							if (metaData.Data && metaData.Data.length > 0) {
								base.LoadDropDownList("#contactCity", metaData.Data);
							} else {
								base.ValidateHasError(metaData, function () {
									alert("Error cargando las ciudades");
								});
							}
						});
						$("#wizardClientForm").validate().element("#contactState");
						$("#wizardClientForm").validate().element("#contactCity");
						$("#wizardClientForm").validate().element("#contactZone");
					});
				},

				DropDownContactCountryChange: function () {
					contact.GetEvent().DropDownContactStateChange();
					$('#contactCountry').change(function () {
						base.ClearDropDownList("#contactState");
						base.ClearDropDownList("#contactCity");
						base.ClearDropDownList("#contactZone");
						$.when(baseHome.GetStatesByCountry($('#contactCountry').val())).then(function (metaData) {
							if (metaData.Data && metaData.Data.length > 0) {
								base.LoadDropDownList("#contactState", metaData.Data);
							} else {
								base.ValidateHasError(metaData, function () {
									alert("Error cargando los estados");
								});
							}
						});
						$("#wizardClientForm").validate().element("#contactCountry");
						$("#wizardClientForm").validate().element("#contactState");
						$("#wizardClientForm").validate().element("#contactCity");
						$("#wizardClientForm").validate().element("#contactZone");
					});
				}
			}
		}
	}
}());

$(function () {
	contact.init();
});