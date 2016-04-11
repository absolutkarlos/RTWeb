var inspection = (function () {
	return {
		init: function () {
			this.LoadDropDownListMaterial("#materialsInspection", base.GetLocalMetaData().Materials.Data);
			this.GetEvent().AddmaterialEvent();
			this.GetEvent().GenerateInspectionEvent();
			this.GetEvent().MaterialInspectionButtomRemoveEvent();
			this.GetEvent().DropDownMaterialsInpectionChange();
			this.GetEvent().InputCatidadEvent();
			$("#materialsInspection").select2();
			this.ValidateShowButtons();
		},

		LoadDropDownListMaterial: function (selector, data) {
			$.each(data, function () {
				$(selector).append($("<option data-unitmeasureid='"+ this.UnitMeasure.Id +"'  data-unitmeasure='"+ this.UnitMeasure.Name +"' />").val(this.Id).text(this.Name));
			});
		},

		InspectionCreate: function (data) {
			$.when(baseInspection.InspectionCreate(data)).then(function (response) {
				home.UpdateOrderPanel();
				home.LoadInfoOrderPanel($("#orderIdLabel").data("orderid"));
				inspection.ValidateShowButtons();
			});
		},

		GetOrderMaterialList: function () {
			var materials = [];
			if (inspection.MaterialsIsValid()) {
				$.each($(".materials"), function (index, item) {
					materials.push($(item).data("ordermaterial"));
				});
			}
			return materials;
		},

		MaterialsIsValid: function () {
			return $(".materials").length > 0;
		},

		ClearMaterials: function () {
			$(".materials").remove();
			$("#materialInspectionListEmpty").show();
		},

		LoadMaterialList: function (materials) {
			if (materials != null && materials.length > 0) {
				$.each(materials, function (index, item) {
					$("#listMetarialInspection").append("<a href='#" + index + "' class='list-group-item materials' style='padding: 0; height: 42px;' data-toggle='collapse' aria-expanded='false'>" +
														'<span class="pull-left" style="height: 100%; padding: 10px;">' + item.Name.toUpperCase() + ' (' + item.Quantity + ')</span>' +
														'<span style="display:none;" data-idremove="' + index + '" class="contact-buttom-remove pull-right"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></span></a>');
				});
				$("#materialInspectionListEmpty").hide();
			}
		},

		GetSiteAccessTypeList: function () {
			var accessTypes = [];
			if ($("#accesstype").val().length > 0) {
				$.each($("#accesstype").val(), function (index, item) {
					accessTypes.push({
						"AccessType.Id": item
					});
				});
			}
			return accessTypes;
		},

		CreateOrderMaterial: function () {
			var material = {
				"Quantity": $("#cantidad").val(),
				"Material.Id": $("#materialsInspection").val()
			}
			
			return JSON.stringify(material);
		},

		ValidateListMaterials: function () {
			if (!inspection.MaterialsIsValid()) {
				$("#materialInspectionListEmpty").addClass("myErrorClass");
				$("#materialInspectionListEmpty").empty();
				$("#materialInspectionListEmpty").html('<span class="text-center" style="height: 100%;">No hay materiales agregados, se requiere al menos 1 material a&ntilde;adido para continuar.</span>');
			}
		},

		ValidateAddMaterial: function () {
			var dropDownMaterialListIsEmpty = $("#materialsInspection").val() === "";
			var inputcantidadIsEmpty = $("#cantidad").val() === "";

			if (dropDownMaterialListIsEmpty) {
				$("#materialsInspection").parent().find(".select2-selection").addClass("select2Error");
			}

			if (inputcantidadIsEmpty) {
				$("#cantidad").addClass("myErrorClass");
			}

			return !dropDownMaterialListIsEmpty && !inputcantidadIsEmpty;
		},

		GetInspection: function (statusId) {
			var inspectionViewModel = {
				"Order.Id": $("#orderIdLabel").data("orderid"),
				"Order.Comments": $("#coment").val(),
				"Order.AditionalCost": $("#costo").val(),
				"Order.SpecialRequirements": $("#requerimientos").val(),
				"Order.OrderStatus.Id": parseInt($("#orderIdLabel").data("orderstatustype")) + 1,
				"Order.InstallationDays": $("#tiempoestimado").val(),
				"Order.Status.Id": statusId,
				"Order.ListMaterials": this.GetOrderMaterialList(),
				"Site.Id": $("#orderIdLabel").data("siteid"),
				"Site.FloorHight": $("#alturapiso").val(),
				"Site.BuildingHight": $("#altura").val(),
				"Site.BuildingFloors": $("#cantidadpisos").val(),
				"Site.ListSiteAccessType": this.GetSiteAccessTypeList(),
				"OrderFlow.Id": $("#statusInspection").data("idorderflow"),
				"IsUpdate": !$("#rechazarInspection").is(":visible") && !$("#modificarInspection").is(":visible")
			}
			return JSON.stringify(inspectionViewModel);
		},

		ValidateShowButtons: function () {
			if ($("#statusInspection").hasClass("glyphicon-ok")) {
				$("#aprobarInspection, #rechazarInspection").hide();
				$("#modificarInspection").show();
			} else if ($("#statusInspection").hasClass("glyphicon-remove")) {
				$("#rechazarInspection").hide();
				$("#aprobarInspection").show();
				$("#modificarInspection").hide();
			} else {
				$("#aprobarInspection, #rechazarInspection").show();
				$("#modificarInspection").hide();
			}
		},

		GetEvent: function () {
			return {
				DropDownMaterialsInpectionChange: function () {
					$('#materialsInspection').change(function () {
						if ($("#materialsInspection").val() === "") {
							$("#materialsInspection").parent().find(".select2-selection").addClass("select2Error");
						} else {
							$("#materialsInspection").parent().find(".select2-selection").removeClass("select2Error");
						}
					});
				},

				InputCatidadEvent: function () {
					$('#cantidad').blur(function () {
						if ($(this).val() === "") {
							$(this).addClass("myErrorClass");
						} else {
							$(this).removeClass("myErrorClass");
						}
					});
				},

				GenerateInspectionEvent: function () {
					$("#rechazarInspection, #aprobarInspection, #modificarInspection").click(function () {
						var statusId = $(this).data("status");
						$("#titleModalAction").text($(this).val());
						inspection.ValidateListMaterials();
						if ($("#wizardInfoForm").valid() && inspection.MaterialsIsValid()) {
							$('#confirmInspection').modal({ backdrop: 'static', keyboard: false }).one('click', '#buttomModalConfirm', function () {
								inspection.InspectionCreate(inspection.GetInspection(statusId));
							});
						}
					});
				},

				AddmaterialEvent: function () {
					$('#addMaterial').click(function () {
						if (inspection.ValidateAddMaterial()) {
							$("#materialInspectionListEmpty").hide();
							$("#materialInspectionListEmpty").empty();
							$("#materialInspectionListEmpty").html('<span class="text-center" style="height: 100%;">No hay materiales agregados</span>');
							var index = $(".materials").length + 1;

							$("#listMetarialInspection").append("<a href='#" + index + "' data-ordermaterial='" + inspection.CreateOrderMaterial() + "' class='list-group-item materials' style='padding: 0; height: 42px;' data-toggle='collapse' aria-expanded='false'>" +
								'<span class="pull-left" style="height: 100%; padding: 10px;">' + $("#materialsInspection option[value='" + $("#materialsInspection").val() + "']").text().toUpperCase() + ' <small style="color: #999999;"> (' + $("#cantidad").val() + ' ' + $("#materialsInspection option[value='" + $("#materialsInspection").val() + "']").data("unitmeasure").toUpperCase() + ') </small></span>' +
								'<span data-idremove="' + index + '" class="materialInspection-buttom-remove pull-right"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></span></a>');

							$("#cantidad").val(1);
							base.ResetDropDownList("#materialsInspection");
							inspection.GetEvent().MaterialInspectionButtomRemoveEvent();
						}
					});
				},

				MaterialInspectionButtomRemoveEvent: function () {
					$(".materialInspection-buttom-remove").unbind("click");
					$(".materialInspection-buttom-remove").click(function () {
						$("#" + $(this).data("idremove")).remove();
						$(this).parent().fadeOut(300, function () {
							$(this).remove();
							if ($(".materials").length === 0) {
								$("#materialInspectionListEmpty").fadeIn(300, function () {
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
	inspection.init();
});