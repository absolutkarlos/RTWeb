var preFactibility = (function () {
	return {
		init: function () {
			this.GetEvent().NOCFileUploadEvent();
			this.GetEvent().GeneratePreFactibilityEvent();
			this.ValidateShowButtons();
		},

		UpdateStatus: function (orderFlowId, orderId, status, radioBaseId, distance, siteId) {
			$.when(basePreFactibility.UpdateStatus(orderFlowId, orderId, status, radioBaseId, distance, siteId)).then(function (result) {
				$("#input-700NOC").fileinput('upload');
			});
		},

		LineSightCreate: function (data) {
			$.when(basePreFactibility.LineSightCreate(data)).then(function (result) {
				$("#input-700NOC").fileinput('upload');
			});
		},

		ValidateComments: function (parameters) {
			var resp = [];
			if ($(".comments").length > 0) {
				$.each($(".comments"), function (index, item) {
					var valid = item.value !== "";
					if (!valid) {
						$(item).addClass("myErrorClass");
						if ($("#errorCommentFileInput").length === 0) {
							$(item).after('<span id="errorCommentFileInput" style="color: red;font-size: 11px;font-style: italic;display: block;margin-top: 5px;">Debe agregar un comentario</span>');
						}
					} else {
						$(item).removeClass("myErrorClass");
						$("#errorCommentFileInput").remove();
					}
					resp.push(valid);
				});
				return !(resp.indexOf(false) > -1);
			}
			return false;
		},

		GetComments: function () {
			var comments = [];
			$.each($(".comments"), function (index, item) {
				comments.push(item.value);
			});
			return JSON.stringify(comments);
		},

		ValidateShowButtons: function () {
			if ($("#user").data("rol") == 4 || $("#user").data("rol") == 6) {
				if ($("#statusPreFactibility").hasClass("glyphicon-ok")) {
					$("#tabInfo").css({ "width": "100%" });
					$("#tabPreFactibilidad").hide();
					$("#wizardInfo").find('.btn-next').hide();
				} else if ($("#statusPreFactibility").hasClass("glyphicon-remove")) {
					$("#tabInfo").css({ "width": "50%" });
					$("#rechazarPreFactibility").hide();
					$("#aprobarPreFactibility").show();
					$("#tabPreFactibilidad").show();
					$("#wizardInfo").find('.btn-next').show();
				} else {
					$("#tabInfo").css({ "width": "50%" });
					$("#aprobarPreFactibility, #rechazarPreFactibility").show();
					$("#tabPreFactibilidad").show();
					$("#wizardInfo").find('.btn-next').show();
				}
			}
		},

		GetPreFactibility: function (statusId) {
			var preFactibilityViewModel = {
				"Order.Id": $("#orderIdLabel").data("orderid"),
				"Order.OrderStatus.Id": parseInt($("#orderIdLabel").data("orderstatustype")) + 1,
				"Order.Status.Id": statusId,
				"OrderFlow.Id": $("#statusPreFactibility").data("idorderflow"),
				"IsUpdate": !$("#rechazarPreFactibility").is(":visible")
			}
			return JSON.stringify(preFactibilityViewModel);
		},

		GetEvent: function () {
			return {
				RadioBaseEvent: function () {
					$("#radioBase").change(function () {
						$("#wizardInfoForm").validate().element("#radioBase");
					});
				},

				BlurComentsEvent: function () {
					$(".comments").blur(function () {
						preFactibility.ValidateComments();
					});
				},

				GeneratePreFactibilityEvent: function () {
					$("#aprobarPreFactibility, #rechazarPreFactibility").click(function () {
						var statusId = $(this).data("status");
						$("#titleModalAction").text($(this).val());
						var formValid = $("#wizardInfoForm").valid();
						var commentsValid = preFactibility.ValidateComments();
						if (formValid && $('#input-700NOC').fileinput('getFileStack').length > 0 && commentsValid) {
							$('#confirmPreFactibility').modal({ backdrop: 'static', keyboard: false }).one('click', '#buttomModalConfirm', function () {
								preFactibility.LineSightCreate(preFactibility.GetPreFactibility(statusId));
							});
						} else {
							if ($('#input-700NOC').fileinput('getFileStack').length === 0) {
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
							}
						}
					});
				},

				NOCFileUploadEvent: function () {
					$('#input-700NOC').on('fileerror', function (event, data, msg) {
						$(".fileinput-remove-button").css({ "height": "38px" });
					});

					$('#input-700NOC').on('fileuploaded', function (event, data, previewId, index) {
						var form = data.form, files = data.files, extra = data.extra,
							response = data.response, reader = data.reader;
						home.UpdateOrderPanel();
						home.LoadInfoOrderPanel(extra.OrderId, false);
						$("#aprobar").prop("disabled", true).css({ "background-color": "#449d44" });
						$("#rechazar").prop("disabled", true).css({ "background-color": "#d9534f" });
						$('#input-700NOC').fileinput('disable');
						$(".btn-file").css({ "background-color": "#337AB7" });
						$(".file-footer-buttons").hide();
					});

					$('#input-700NOC').on('fileselect', function (event) {
						preFactibility.GetEvent().BlurComentsEvent();
						if ($('#input-700NOC').fileinput('getFileStack').length === 0) {
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
							var metaData = base.GetLocalMetaData();
							if ((metaData.RadioBase) && (metaData.RadioBase.Data) && (metaData.RadioBase.Data.length > 0)) {
								base.LoadDropDownList(".radioBase", metaData.RadioBase.Data);
							}
							home.GetEvent().Select2Event();
							preFactibility.GetEvent().RadioBaseEvent();
						}
						$(".fileinput-remove-button").css({ "height": "38px" });
					});
					$("#input-700NOC").fileinput({
						language: "es",
						uploadUrl: base.GetRootUploadFile(), // server upload action
						uploadAsync: true,
						maxFileCount: 1,
						resizePreference: 'width',
						resizeImage: true,
						allowedFileExtensions: ['jpeg', 'jpg', 'gif', 'png'],
						showUpload: false,
						browseLabel: "",
						browseIcon: "<img src='/Content/Images/Icons/ic_add_a_photo_white_24dp_1x.png' />",
						previewTemplates: {
							image: '<div class="file-preview-frame" id="{previewId}" data-fileindex="{fileindex}" style="width: 350px;">\n' +
								'   <img src="{data}" class="file-preview-image" title="{caption}" alt="{caption}" >\n' +
								'   {footer}\n' +
								'</div>\n',
							other: '<div class="file-preview-frame{frameClass}" id="{previewId}" data-fileindex="{fileindex}"' +
									' title="{caption}">\n' +
									'   <div class="file-preview-other-frame">\n' +
									'     <div class="file-preview-other">\n' +
									'       <span class="{previewFileIconClass}">{previewFileIcon}</span>\n' +
									'     </div>\n' +
									'   </div>\n' +
									'    <div class="file-caption-name" style="width:{width}">{caption}</div>\n' +
									'<div class="file-actions">\n' +
									'    <div class="file-footer-buttons">\n' +
									'       <button type="button" class="kv-file-remove btn btn-xs btn-default" title="Eliminar archivo"{dataUrl}{dataKey}><i class="glyphicon glyphicon-trash text-danger"></i></button>\n' +
									'    </div>\n' +
									'    <div class="file-upload-indicator" tabindex="-1" title="">{indicator}</div>\n' +
									'    <div class="clearfix"></div>\n' +
									'</div>\n' +
									'</div>'
						},
						uploadExtraData: function (previewId, index) {
							return {
								OrderNumber: $("#orderIdLabel").data("ordernumber") + "_SHOT-" + "1",
								OrderId: $("#orderIdLabel").data("orderid"),
								SiteId: $("#orderIdLabel").data("siteid"),
								OrderShotType: 1,
								LinkType: $("#" + previewId).find("select[name~='linkType']").val(),
								RadioBaseId: $("#" + previewId).find("select[name~='radioBase']").val(),
								Distance: $("#" + previewId).find("input[name~='distance']").val(),
								Comment: $("#" + previewId).find("textarea").val()
							};
						},
						layoutTemplates: {
							footer: '<div class="file-thumbnail-footer">\n' +
								'    <div class="file-caption-name" style="width:{width}">{caption}</div>\n' +
								'    <div class="form-group" style="margin-top: 5px;text-align: left;"><label style="display: block;"> RADIO BASE <small>(requerido)</small></label>\n' +
								'    <select style="width: 100%;" id="radioBase" name="radioBase" class="form-control select2 radioBase"><option value="">Seleccione una radio base</option></select>\n' +
								'    <label style="display: block; margin-top: 15px;"> DISTANCIA EN KILOMETROS <small>(requerido)</small></label>\n' +
								'    <input style="width: 100%;" id="distance" name="distance" class="form-control" placeholder="Distancia..." />\n' +
								'    <label style="display: block; margin-top: 15px;"> TIPO DE ENLACE </label>\n' +
								'    <select style="width: 100%;" id="linkType" name="linkType" class="form-control">\n' +
								'    <option value="Punto a Punto">Punto a Punto</option><option value="Multipunto">Multipunto</option></select></div>\n' +
								'    <div class="form-group" style="margin-top: 5px;text-align: left;"><label> COMENTARIO <small>(requerido)</small></label>\n' +
								'    <textarea style="width: 100%; resize: none;" name="Comment" class="form-control comments" placeholder="Comentario..." rows="3" cols="15" aria-invalid="false"></textarea></div>\n' +
								'    {progress} {actions}\n' +
								'</div>',
							actions: '<div class="file-actions">\n' +
								'    <div class="file-footer-buttons">\n' +
								'       {delete}' +
								'    </div>\n' +
								'    <div class="file-upload-indicator" tabindex="-1" title="{indicatorTitle}">{indicator}</div>\n' +
								'    <div class="clearfix"></div>\n' +
								'</div>'
						}
					});
					$(".fileinput-remove-button").css({ "border-left": "1px solid #DDD", "border-right": "2px solid #DDD" });
					$(".file-caption").css({ "height": "38px" });
				}
			}
		}
	}
}());

//$(function () {
//	preFactibility.init();
//});