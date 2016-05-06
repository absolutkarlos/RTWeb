var site = (function () {
	return {
		init: function() {
			this.GetEvent().AddScheduleEvent();
			$(".js-example-basic-multiple").select2();
		},

		ClearSiteSchedules: function () {
			$(".siteschedules").remove();
			$("#siteScheduleListEmpty").show();
		},

		ResetSiteSchedule: function () {
			$("#startime").val("8:00 AM");
			$("#endtime").val("5:00 PM");
			$.each($(".week:checked"), function (index, item) {
				$(item).parent().removeClass('active');
				$(item).prop("checked", false);
			});
		},

		ResetForm: function () {
			this.ClearSiteSchedules();
			$("#startime").val("8:00 AM");
			$("#endtime").val("5:00 PM");
			$("#broadband").val("");
			$("#longitude").val("");
			$("#latitude").val("");
			$("#coords").val("");
			$("#siteName").val("");
			$("#sitedetailedadress").val("");
			$(".js-example-basic-multiple").val(0).trigger("change");
			$.each($(".week:checked"), function (index, item) {
				$(item).parent().removeClass('active');
				$(item).prop("checked", false);
			});
			base.InitializeGoogleMap();
		},

		GetSiteScheduleList: function () {
			var siteSchedules = [];
			if (site.SchedulesIsValid()) {
				$.each($(".siteschedules"), function (index, item) {
					$.each($(item).data("schedule"), function (index2, item2) {
						siteSchedules.push(item2);
					});
				});
			}
			return siteSchedules;
		},

		GetServiceTypeList: function () {
			var serviceTypes = [];
			if ($("#servicetype").val().length > 0) {
				$.each($("#servicetype").val(), function (index, item) {
					serviceTypes.push({
						"Id": item
					});
				});
			}
			return serviceTypes;
		},

		CalculateBandWidth: function() {
			return parseFloat($("#broadband").val() * Math.pow(1000, $("#unittype").val())).toString();
		},

		GetSite: function () {
			var siteViewModel = {
				"CountryAbbrevation": base.GetCountryAbbrevation(),
				"Site.Name": $("#siteName").val(),
				"Site.Client.Id": client.GetClientId(),
				"Site.BandWidth": this.CalculateBandWidth(),
				"Site.Longitude": $("#longitude").val(),
				"Site.Latitude": $("#latitude").val(),
				"Site.Address": $("#sitedetailedadress").val(),
				"Site.ListSiteSchedule": this.GetSiteScheduleList(),
				"Site.ListServiceType": this.GetServiceTypeList()
			}
			return JSON.stringify(siteViewModel);
		},

		AddSiteScheduleToLocalStorage: function () {
			var list = new Array();
			$.each($(".week:checked"), function (index, item) {
				list.push({
					"Day": item.value,
					"StartTimeString": $("#startime").val(),
					"EndTimeString": $("#endtime").val()
				});
			});
			$("#siteScheduleListEmpty").hide();
			return JSON.stringify(list);
		},

		SiteCreate: function () {
			return baseSite.SiteCreate(this.GetSite()).success(function (response) {
				return base.ValidateHasError(response, function () {
					alert("error creando la orden");
				});
			});
		},

		SchedulesIsValid: function () {
			return $(".siteschedules").length > 0;
		},

		AddSchedulesIsValid: function () {
			return $(".week:checked").length > 0;
		},

		GetWeekDays: function () {
			var days = "";
			var length = $(".week:checked").length;
			$.each($(".week:checked"), function (index, item) {
				if ((index > 0) && (index === (length - 1))) {
					days += " y ";
				} else if (index > 0) {
					days += ", ";
				}

				days += $.trim($(item).parent().text());
			});
			return days;
		},

		GetEvent: function () {
			return {

				CreteOrderButtomEvent: function() {
					$("#finish").click(function () {
						if (validateThirdStep()) {
							home.UpdateOrderPanel();
							$("#modal").find(".modal-body").empty();
							$("#modal").find(".modal-body").append("<h5>La orden se ha <b>generado</b> satisfactoriamente</h5>");
							$('#modal').modal('show');
						}
					});
				},

				AddScheduleEvent: function () {
					$("#addSchedule").click(function () {
						if (site.AddSchedulesIsValid()) {

							var schedule = site.AddSiteScheduleToLocalStorage();

							$("#listSchedule").append("<div class='form-group siteschedules' data-schedule='" + schedule + "' style='display: table; width: 100%;'><label class='Infolabel' style='height: 50px; display: table-cell; vertical-align: bottom;'>" +
								'<b>' + $("#startime").val() + '</b> hasta las <b>' + $("#endtime").val() + '</b><br/> ' + site.GetWeekDays() + '<small style="margin-left: 5px;">(Dias de la Semana)</small>' +
								'</label><span class="contact-buttom-remove"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></span>' +
								'</div>');

							site.GetEvent().SiteScheduleButtomRemoveEvent();
							site.ResetSiteSchedule();
						} else {
							$("#siteScheduleListEmpty").empty().html('<label class="Infolabel" style="height: 50px; display: table-cell; vertical-align: bottom;">No hay horarios agregados, para continuar debe tener a&ntilde;adido al menos un horario.</label>');
							$("#siteScheduleListEmpty").css({ color: "red" });
							$("#siteScheduleListEmpty").find(".Infolabel").css({ "border-bottom-color": "red" });
						}
					});
				},

				SiteScheduleButtomRemoveEvent: function () {
					$(".contact-buttom-remove").unbind("click");
					$(".contact-buttom-remove").click(function () {
						$(this).parent().fadeOut(300, function () {
							$(this).remove();
							if ($(".siteschedules").length === 0) {
								$("#siteScheduleListEmpty").css({ color: "black" });
								$("#siteScheduleListEmpty").find(".Infolabel").css({ "border-bottom-color": "black" });
								$("#siteScheduleListEmpty").find(".Infolabel").empty().text("No hay horarios agregados");
								$("#siteScheduleListEmpty").fadeIn(300, function () {
									$(this).show();
								});
							}
						});
					});
				}
			}
		}
	}
}());

$(function () {
	site.init();
});