searchVisible = 0;
transparent = true;
var $validator;
var $validatorInfoForm;

$(document).ready(function () {
	/*  Activate the tooltips      */
	$('[rel="tooltip"]').tooltip();

	$validatorInfoForm = $("#wizardInfoForm").validate({
		onkeyup: false,
		errorClass: "myErrorClass",

		errorPlacement: function (error, element) {
			var elem = $(element);
			if (elem.is("select")) {
				var container = $(element).parent().find(".select2-container");
				if (container != null) {
					error.insertAfter(container);
				} else {
					error.insertAfter(element);
				}
			} else {
				error.insertAfter(element);
			}
		},


		highlight: function (element, errorClass, validClass) {
			var elem = $(element);
			if (elem.is("select")) {
				var container = $(element).parent().find(".select2-selection");
				if (container != null) {
					container.addClass("select2Error");
				} else {
					elem.addClass(errorClass);
				}
			} else {
				elem.addClass(errorClass);
			}
		},

		//When removing make the same adjustments as when adding
		unhighlight: function (element, errorClass, validClass) {
			var elem = $(element);
			if (elem.is("select")) {
				var container = $(element).parent().find(".select2-selection");
				if (container != null) {
					container.removeClass("select2Error");
				} else {
					elem.removeClass(errorClass);
				}
			} else {
				elem.removeClass(errorClass);
			}
		},
		rules: {
			distance: "required",
			radioBase: "required",
			alturapiso: {
				required: true,
				pattern: /^\d+$/
			},
			altura: {
				required: true,
				pattern: /^\d+$/
			},
			accesstype: "required",
			coment: "required"
		},
		messages: {
				distance: "Ingrese la distancia entre el sitio y la celda seleccionada",
				radioBase: "Seleccione una celda",
				alturapiso: {
					required: "Ingrese la altura de los pisos del edificio",
					pattern: "La altura de los pisos del edificio solo debe contener numeros"
				},
				altura: {
					required: "Ingrese la altura del edificio",
					pattern: "La altura del edificio solo debe contener numeros"
				},
				accesstype: "Seleccione un tipo de acceso",
				coment: "Ingrese un comentario"
			}
	});

	$validator = $("#wizardClientForm").validate({
		onkeyup: false,
		errorClass: "myErrorClass",

		errorPlacement: function (error, element) {
			var elem = $(element);
			if (elem.is("select")) {
				var container = $(element).parent().find(".select2-container");
				if (container != null) {
					error.insertAfter(container);
				} else {
					error.insertAfter(element);
				}
			} else {
				error.insertAfter(element);
			}
		},


		highlight: function (element, errorClass, validClass) {
			var elem = $(element);
			if (elem.is("select")) {
				var container = $(element).parent().find(".select2-selection");
				if (container != null) {
					container.addClass("select2Error");
				} else {
					elem.addClass(errorClass);
				}
			} else {
				elem.addClass(errorClass);
			}
		},

		//When removing make the same adjustments as when adding
		unhighlight: function (element, errorClass, validClass) {
			var elem = $(element);
			if (elem.is("select")) {
				var container = $(element).parent().find(".select2-selection");
				if (container != null) {
					container.removeClass("select2Error");
				} else {
					elem.removeClass(errorClass);
				}
			} else {
				elem.removeClass(errorClass);
			}
		},

		rules: {
			fullname: "required",
			businessname: "required",
			ruc: "required",
			detailedadress: "required",
			clienttype: "required",
			country: "required",
			state: "required",
			city: "required",
			zone: "required",
			contactfullname: "required",
			position: "required",
			phone: {
				required: true,
				pattern: /^\+([0-9]{10})|(\([0-9]{3}\)\s+[0-9]{3}\-[0-9]{4})/
			},
			email: {
				required: true,
				email: true
			},
			contactCountry: "required",
			contactState: "required",
			contactCity: "required",
			contactZone: "required",
			latitude: "required",
			longitude: "required",
			siteName: "required",
			broadband: {
				required: true,
				pattern: /^\d+$/
			},
			servicetype: "required",
			sitedetailedadress: "required"
		},
		messages: {
			fullname: "Ingrese el nombre de la marca.",
			businessname: "Ingrese el nombre del negocio.",
			ruc: "Ingrese el ruc o rif",
			detailedadress: "Ingrese la direcci&oacute;n",
			clienttype: "Seleccione el tipo de cliente",
			country: "Seleccione el pa&iacute;s",
			state: "Seleccione el estado",
			city: "Seleccione la ciudad",
			zone: "Seleccione la zona",
			contactfullname: "Ingrese el nombre de la marca.",
			position: "Ingrese el nombre del negocio.",
			phone: {
				required: "Ingrese un numero de telefono",
				pattern: "Su numero de telefono debe tener 8 caracteres"
			},
			email: {
				required: "Ingrese una direcci&oacute;n de email",
				email: "Ingrese una direcci&oacute;n de email valida"
			},
			contactCountry: "Seleccione el pa&iacute;s",
			contactState: "Seleccione el estado",
			contactCity: "Seleccione la ciudad",
			contactZone: "Seleccione la zona",
			latitude: "Ingrese la latitud de las coordenasdas de ubicaci&oacute;n",
			longitude: "Ingrese la longitud de las coordenasdas de ubicaci&oacute;n",
			siteName: "Ingrese el nombre del sitio",
			broadband: {
				required: "Ingrese un ancho de banda",
				pattern: "El ancho de banda solo debe contener numeros."
			},
			servicetype: "Seleccione el tipo de servicio",
			sitedetailedadress: "Ingrese la direcci&oacute;n detallada"
		}
	});

	$('.wizard-card').bootstrapWizard({
		'tabClass': 'nav nav-pills',
		'nextSelector': '.btn-next',
		'previousSelector': '.btn-previous',

		onInit: function (tab, navigation, index) {

			//check number of tabs and fill the entire row
			var $total = navigation.find('li').length;
			$width = 100 / $total;

			$display_width = $(document).width();

			if ($display_width < 600 && $total > 3) {
				$width = 50;
			}

			navigation.find('li').css('width', $width + '%');
		},
		onNext: function (tab, navigation, index) {
			if (index === 1) {
				if (navigation.context.id === "wizardInfo") {
					return true;
				} else {
					if ($(".readOnly").is(":visible")) {
						return true;
					}
					return validateFirstStep();
				}
			} else if (index === 2) {
				if (navigation.context.id === "wizardInfo") {
					return true;
				} else {
					return validateSecondStep();
				}
			} else if (index === 3) {
				return validateThirdStep();
			}
			return true;
		},
		onTabClick: function (tab, navigation, index) {
			// Disable the posibility to click on tabs
			return false;
		},
		onTabShow: function (tab, navigation, index) {
			var $total = navigation.find('li').length;
			var $current = index + 1;

			var wizard = navigation.closest('.wizard-card');

			// If it's the last tab then hide the last button and show the finish instead
			if ($current >= $total && $total > 1) {
				$(wizard).find('.btn-next').hide();
				if (navigation.context.id !== "wizardInfo") {
					$(wizard).find('.btn-finish').show();
				}
			} else if ($total > 1) {
				if ((navigation.context.id === "wizardInfo") && (index === 1) && !$("#statusInspection").hasClass("glyphicon-ok")) {
					$(wizard).find('.btn-next').hide();
				} else {
					$(wizard).find('.btn-next').show();
				}
				$(wizard).find('.btn-finish').hide();
			} else {
				$(wizard).find('.btn-finish').hide();
				$(wizard).find('.btn-next').hide();
			}

			if (index == 2) {
				base.RefreshMap();
			}
		}
	});

	site.GetEvent().CreteOrderButtomEvent();

	$(".js-example-basic-multiple").on("change", function () {
		var form;
		if ($("#wizardClientForm").length) {
			form = $("#wizardClientForm");
		} else {
			form = $("#wizardInfoForm");
		}
		form.validate().element("#" + $(this).prop("id"));
	});

	// Prepare the preview for profile picture
	$("#wizard-picture").change(function () {
		readURL(this);
	});

	$('[data-toggle="wizard-radio"]').click(function () {
		wizard = $(this).closest('.wizard-card');
		wizard.find('[data-toggle="wizard-radio"]').removeClass('active');
		$(this).addClass('active');
		$(wizard).find('[type="radio"]').removeAttr('checked');
		$(this).find('[type="radio"]').attr('checked', 'true');
	});

	$('[data-toggle="wizard-checkbox"]').click(function () {
		if ($(this).hasClass('active')) {
			$(this).removeClass('active');
			$(this).find('[type="checkbox"]').removeAttr('checked');
		} else {
			$(this).addClass('active');
			$(this).find('[type="checkbox"]').prop('checked', true);
			$(this).find('[type="checkbox"]')[0].checked = true;
		}
	});

	$height = $(document).height();
	$('.set-full-height').css('height', $height);
});

function validateFirstStep() {
	var valid = $("#wizardClientForm").valid();
	var hasError = true;
	if (valid) {
		var resp = client.ClientCreate();
		hasError = resp.responseJSON.ErrorMessage !== null;
		if (!hasError) {
			client.ShowContentReadOnly();
		}
	}
	return valid && !hasError;
	//return true;
}

function validateSecondStep() {
	if (!contact.ContactsIsValid()) {
		$("#contactListEmpty").addClass("myErrorClass");
		$("#contactListEmpty").empty();
		$("#contactListEmpty").html('<span class="text-center" style="height: 100%;">No hay contactos agregados, se requiere al menos 1 contacto a&ntilde;adido para continuar con el siguiente paso.</span>');
		return false;
	} else {
		$("#contactListEmpty").empty();
		$("#contactListEmpty").html('<span class="text-center" style="height: 100%;">No hay contactos agregados</span>');
		return true;
	}
	//return true;
}

function validateThirdStep() {
	var valid = $("#wizardClientForm").valid();

	if (!site.SchedulesIsValid()) {
		$("#siteScheduleListEmpty").empty().html('<label class="Infolabel" style="height: 50px; display: table-cell; vertical-align: bottom;">No hay horarios agregados, para continuar debe tener a&ntilde;adido al menos un horario.</label>');
		$("#siteScheduleListEmpty").css({ color: "red" });
		$("#siteScheduleListEmpty").find(".Infolabel").css({ "border-bottom-color": "red" });
		valid = false;
	} else {
		$("#siteScheduleListEmpty").css({ color: "black" });
		$("#siteScheduleListEmpty").find(".Infolabel").css({ "border-bottom-color": "black" });
	}

	var hasError = true;
	if (valid) {
		var resp = site.SiteCreate();
		hasError = resp.responseJSON.ErrorMessage !== null;
	};

	if (!hasError) {
		site.ResetForm();
		contact.ClearContacts();
		$validator.resetForm();
		$('#wizardClient').bootstrapWizard('show', 0);
	}

	return valid && !hasError;
}

//Function to show image before upload

function readURL(input) {
	if (input.files && input.files[0]) {
		var reader = new FileReader();

		reader.onload = function (e) {
			$('#wizardPicturePreview').attr('src', e.target.result).fadeIn('slow');
		}
		reader.readAsDataURL(input.files[0]);
	}
}