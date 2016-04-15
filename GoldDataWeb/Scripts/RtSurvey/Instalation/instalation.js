var instalation = (function () {
	return {
		init: function () {
			this.LoadDropDownListMaterial("#materialsInstalation", base.GetLocalMetaData().Materials.Data);
			this.GetEvent().AddmaterialEvent();
			this.GetEvent().GenerateInstalationEvent();
			this.GetEvent().MaterialInstalationButtomRemoveEvent();
			this.GetEvent().DropDownMaterialsInstalationChange();
			this.GetEvent().InputCatidadEvent();
			this.GetEvent().InstFileUploadEvent();
			$("#materialsInstalation").select2();
			this.ValidateShowButtons();
		},

		LoadInstalationPanel: function (id) {
			$("#refreshInstalationPanel").css({ "display": "table" });
			$("#contentPanelInstalation").hide();
			$.when(baseInstalation.LoadInstalationPanel(id)).then(function (panel) {
				var metaData = base.GetLocalMetaData();
				$("#contentPanelInstalation").remove();
				$("#instalation").html(panel);
				$("#refreshInstalationPanel").css({ "display": "none" });
				$("#contentPanelInstalation").show();
				instalation.LoadDropDownListMaterial("#materialsInstalation", metaData.Materials.Data);
				instalation.GetEvent().AddmaterialEvent();
				instalation.GetEvent().GenerateInstalationEvent();
				instalation.GetEvent().MaterialInstalationButtomRemoveEvent();
				instalation.GetEvent().DropDownMaterialsInstalationChange();
				instalation.GetEvent().InputCatidadEvent();
				instalation.GetEvent().InstFileUploadEvent();
				$("#materialsInstalation").select2();
				instalation.ValidateShowButtons();
			});
		},

		GetOrderShots: function () {
			var orderShots = [];
			if (typeof listOrdersShotsInstalation != "undefined") {
				orderShots = JSON.parse(listOrdersShotsInstalation);
			}

			var initialPreview = [];
			$.each(orderShots, function (index, item) {
				var caption = item.shotpath.split('/')[4];
				initialPreview.push("<img style='display: table;margin-left: auto;margin-right: auto;' src='" + item.shotpath + "' class='file-preview-image' alt='" + caption + "' title='" + caption + "'>\n <div class='file-caption-name'>" + caption + "</div>" +
									"<div class=\"file-actions\">\n" +
									'    <div class="file-footer-buttons">\n' +
									'       <button type="button" class="kv-file-remove btn btn-xs btn-default" title="Eliminar archivo"{dataUrl}{dataKey}><i class="glyphicon glyphicon-trash text-danger"></i></button>\n' +
									'    </div>\n' +
									'    <div class="clearfix"></div>\n' +
									'</div>');
			});
			return initialPreview;
		},

		LoadDropDownListMaterial: function (selector, data) {
			$.each(data, function () {
				$(selector).append($("<option data-unitmeasureid='" + this.UnitMeasure.Id + "'  data-unitmeasure='" + this.UnitMeasure.Name + "' />").val(this.Id).text(this.Name));
			});
		},

		InstalationCreate: function (data) {
			$.when(baseInstalation.InstalationCreate(data)).then(function (response) {
				home.UpdateOrderPanel();
				home.LoadInfoOrderPanel($("#orderIdLabel").data("orderid"));
				instalation.ValidateShowButtons();
			});
		},

		GetOrderMaterialList: function () {
			var materials = [];
			if (instalation.MaterialsIsValid()) {
				$.each($(".materialsInst"), function (index, item) {
					materials.push($(item).data("ordermaterial"));
				});
			}
			return materials;
		},

		MaterialsIsValid: function () {
			return $(".materialsInst").length > 0;
		},

		ClearMaterials: function () {
			$(".materialsInst").remove();
			$("#materialInstalationListEmpty").show();
		},

		LoadMaterialList: function (materials) {
			if (materials != null && materials.length > 0) {
				$.each(materials, function (index, item) {
					$("#listMetarialInstalation").append("<a href='#" + index + "' class='list-group-item materialsInst' style='padding: 0; height: 42px;' data-toggle='collapse' aria-expanded='false'>" +
														'<span class="pull-left" style="height: 100%; padding: 10px;">' + item.Name.toUpperCase() + ' (' + item.Quantity + ')</span>' +
														'<span style="display:none;" data-idremove="' + index + '" class="contact-buttom-remove pull-right"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></span></a>');
				});
				$("#materialInstalationListEmpty").hide();
			}
		},

		CreateOrderMaterial: function () {
			var material = {
				"Quantity": $("#cantidadInstalation").val(),
				"Material.Id": $("#materialsInstalation").val()
			}

			return JSON.stringify(material);
		},

		ValidateListMaterials: function () {
			if (!instalation.MaterialsIsValid()) {
				$("#materialInstalationListEmpty").addClass("myErrorClass");
				$("#materialInstalationListEmpty").empty();
				$("#materialInstalationListEmpty").html('<span class="text-center" style="height: 100%;">No hay materiales agregados, se requiere al menos 1 material a&ntilde;adido para continuar.</span>');
			}
			return instalation.MaterialsIsValid();
		},

		ValidateAddMaterial: function () {
			var dropDownMaterialListIsEmpty = $("#materialsInstalation").val() === "";
			var inputcantidadIsEmpty = $("#cantidadInstalation").val() === "";

			if (dropDownMaterialListIsEmpty) {
				$("#materialsInstalation").parent().find(".select2-selection").addClass("select2Error");
			}

			if (inputcantidadIsEmpty) {
				$("#cantidadInstalation").addClass("myErrorClass");
			}

			return !dropDownMaterialListIsEmpty && !inputcantidadIsEmpty;
		},

		GetInstalation: function (statusId) {
			var instalationViewModel = {
				"Order.Id": $("#orderIdLabel").data("orderid"),
				"Order.SettingUp": $("#detail").val(),
				"Order.OrderStatus.Id": parseInt($("#orderIdLabel").data("orderstatustype")) + 1,
				"Order.Status.Id": statusId,
				"Order.ListMaterials": this.GetOrderMaterialList(),
				"OrderFlow.Id": $("#statusInstalation").data("idorderflow"),
				"IsUpdate": !$("#rechazarInstalation").is(":visible") && !$("#modificarInstalation").is(":visible")
			}
			return JSON.stringify(instalationViewModel);
		},

		ValidateShowButtons: function () {
			if ($("#statusInstalation").hasClass("glyphicon-ok")) {
				$("#aprobarInstalation, #rechazarInstalation").hide();
				$("#modificarInstalation").show();
			} else if ($("#statusInstalation").hasClass("glyphicon-remove")) {
				$("#rechazarInstalation").hide();
				$("#aprobarInstalation").show();
				$("#modificarInstalation").hide();
			} else {
				$("#aprobarInstalation, #rechazarInstalation").show();
				$("#modificarInstalation").hide();
			}
		},

		GetEvent: function () {
			return {
				DropDownMaterialsInstalationChange: function () {
					$('#materialsInstalation').change(function () {
						if ($("#materialsInstalation").val() === "") {
							$("#materialsInstalation").parent().find(".select2-selection").addClass("select2Error");
						} else {
							$("#materialsInstalation").parent().find(".select2-selection").removeClass("select2Error");
						}
					});
				},

				InputCatidadEvent: function () {
					$('#cantidadInstalation').blur(function () {
						if ($(this).val() === "") {
							$(this).addClass("myErrorClass");
						} else {
							$(this).removeClass("myErrorClass");
						}
					});
				},

				GenerateInstalationEvent: function () {
					$("#rechazarInstalation, #aprobarInstalation, #modificarInstalation").click(function () {
						var statusId = $(this).data("status");
						$("#titleModalActionInst").text($(this).val());
						var formValid = $("#wizardInfoForm").valid();
						var materialsValid = instalation.ValidateListMaterials();
						if (formValid && materialsValid) {
							$('#confirmInstalation').modal({ backdrop: 'static', keyboard: false }).one('click', '#buttomModalConfirmInst', function () {
								instalation.InstalationCreate(instalation.GetInstalation(statusId));
							});
						}
					});
				},

				AddmaterialEvent: function () {
					$('#addMaterialInstalation').click(function () {
						if (instalation.ValidateAddMaterial()) {
							$("#materialInstalationListEmpty").hide();
							$("#materialInstalationListEmpty").empty();
							$("#materialInstalationListEmpty").html('<span class="text-center" style="height: 100%;">No hay materiales agregados</span>');
							var index = $(".materialsInst").length + 1;

							$("#listMetarialInstalation").append("<a href='#" + index + "' data-ordermaterial='" + instalation.CreateOrderMaterial() + "' class='list-group-item materialsInst' style='padding: 0; height: 42px;' data-toggle='collapse' aria-expanded='false'>" +
								'<span class="pull-left" style="height: 100%; padding: 10px;">' + $("#materialsInstalation option[value='" + $("#materialsInstalation").val() + "']").text().toUpperCase() + ' <small style="color: #999999;"> (' + $("#cantidadInstalation").val() + ' ' + $("#materialsInstalation option[value='" + $("#materialsInstalation").val() + "']").data("unitmeasure").toUpperCase() + ') </small></span>' +
								'<span data-idremove="' + index + '" class="materialInstalation-buttom-remove pull-right"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></span></a>');

							$("#cantidadInstalation").val(1);
							base.ResetDropDownList("#materialsInstalation");
							instalation.GetEvent().MaterialInstalationButtomRemoveEvent();
						}
					});
				},

				MaterialInstalationButtomRemoveEvent: function () {
					$(".materialInstalation-buttom-remove").unbind("click");
					$(".materialInstalation-buttom-remove").click(function () {
						$("#" + $(this).data("idremove")).remove();
						$(this).parent().fadeOut(300, function () {
							$(this).remove();
							if ($(".materialsInst").length === 0) {
								$("#materialInstalationListEmpty").fadeIn(300, function () {
									$(this).show();
								});
							}
						});
					});
				},

				InstFileUploadEvent: function () {
					//$('#input-700Instalation').on('fileuploaded', function (event, data, previewId, index) {
					//	var form = data.form, files = data.files, extra = data.extra,
					//		response = data.response, reader = data.reader;
					//	home.UpdateOrderPanel();
					//	home.LoadInfoOrderPanel(extra.OrderId, false);
					//	$("#aprobar").prop("disabled", true).css({ "background-color": "#449d44" });
					//	$("#rechazar").prop("disabled", true).css({ "background-color": "#d9534f" });
					//	$('#input-700NOC').fileinput('disable');
					//	$(".btn-file").css({ "background-color": "#337AB7" });
					//	$(".file-footer-buttons").hide();
					//});

					$('#input-700Instalation').on('fileselect', function (event) {
						if ($('#input-700Instalation').fileinput('getFileStack').length === 0) {
							$("#errorFileInput").remove();
							$(".file-input").after("<span id='errorFileInput' style='color: red;font-size: 11px;font-style: italic;display: block;margin-top: 5px;'>Debe agregar una imagen</span>");
							$(".file-caption").css({
								"border-width": "1px",
								"border-style": "solid",
								"border-color": "#cc0000",
								"background-color": "#f3d8d8",
								"background-image": "url(http://goo.gl/GXVcmC)",
								"background-position": "50% 50%",
								"background-repeat": "repeat"
							});
						} else {
							$("#errorFileInput").remove();
							$(".file-caption").css({
								"background-color": "#fff",
								"background-image": "none",
								"border": "1px solid #ccc"
							});
						}
						$(".fileinput-remove-button").css({ "height": "38px" });
					});

					$("#input-700Instalation").fileinput({
						language: "es",
						uploadUrl: base.GetRootUploadFile(), // server upload action
						uploadAsync: true,
						maxFileCount: 3,
						allowedFileExtensions: ['jpeg', 'jpg', 'gif', 'png'],
						showUpload: false,
						browseLabel: "",
						browseIcon: "<img src='/Content/Images/Icons/ic_add_a_photo_white_24dp_1x.png' />",
						initialPreview: instalation.GetOrderShots(),
						overwriteInitial: false,
						initialPreviewShowDelete: true,
						//previewTemplates: {
						//	image: '<div class="file-preview-frame" id="{previewId}" data-fileindex="{fileindex}">\n' +
						//		'   <img src="{data}" class="file-preview-image" title="{caption}" alt="{caption}" style="width:100%;height:400px">\n' +
						//		'   {footer}\n' +
						//		'</div>\n'
						//},
						uploadExtraData: function (previewId, index) {
							return {
								OrderNumber: $("#orderIdLabel").data("ordernumber") + "_SHOT-" + "2",
								OrderId: $("#orderIdLabel").data("orderid"),
								OrderShotCount: parseInt($("#orderIdLabel").data("ordershotcount")) + (index + 1),
								OrderShotType: 2
								//Comment: $("#" + previewId).find("textarea").val()
							};
						},
						//layoutTemplates: {
						//	footer: '<div class="file-thumbnail-footer">\n' +
						//		'    <div class="file-caption-name" style="width:{width}">{caption}</div>\n' +
						//		'    <div class="form-group" style="margin-top: 5px;text-align: left;"><label style="display: block;"> RADIO BASE <small>(requerido)</small></label>\n' +
						//		'    <select style="width: 100%;" id="radioBase" name="radioBase" class="form-control select2"><option value="">Seleccione una radio base</option></select>\n' +
						//		'    <label style="display: block; margin-top: 15px;"> DISTANCIA <small>(requerido)</small></label>\n' +
						//		'    <input style="width: 100%;" id="distance" name="distance" class="form-control" placeholder="Distancia..." /></div>\n' +
						//		'    <div class="form-group" style="margin-top: 5px;text-align: left;"><label> COMENTARIO <small>(requerido)</small></label>\n' +
						//		'    <textarea style="width: 100%; resize: none;" name="Comment" class="form-control comments" placeholder="Comentario..." rows="3" cols="15" aria-invalid="false"></textarea></div>\n' +
						//		'    {progress} {actions}\n' +
						//		'</div>',
						//	actions: '<div class="file-actions">\n' +
						//		'    <div class="file-footer-buttons">\n' +
						//		'       {delete}' +
						//		'    </div>\n' +
						//		'    <div class="file-upload-indicator" tabindex="-1" title="{indicatorTitle}">{indicator}</div>\n' +
						//		'    <div class="clearfix"></div>\n' +
						//		'</div>'
						//}
					});
					$(".fileinput-remove-button").css({ "border-left": "1px solid #DDD", "border-right": "2px solid #DDD" });
					$(".file-caption").css({ "height": "38px" });
					instalation.ValidateShowButtons();
				}
			}
		}
	}
}());

//$(function () {
//	instalation.init();
//});