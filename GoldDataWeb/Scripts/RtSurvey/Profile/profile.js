var profile = (function () {

	return {
		init: function () {
			this.GetEvent().ValidateProfile();
			this.GetEvent().Update();
			base.InitializeCarousel();
			base.ValidateExpireToken();
		},

		IsValid: function () {
			return $("#profileForm").valid();
		},

		GetUserData: function () {
			var userData = {
				"UserName": $("#user").val(),
				"Email": $("#email").val(),
				"Name": $("#name").val(),
				"PhoneNumber": $("#phone").val(),
				"BillingAddress": $("#address").val(),
				"ZipCode": $("#zipcode").val(),
				"Password": $("#newPassword").val(),
				"Status.Id": 1
			}

			return JSON.stringify(userData);
		},

		UpdateProfile: function () {
			baseProfile.UpdateProfile(profile.GetUserData()).success(function (response) {
				alert("Success");
				$("#newPassword").val("");
				$("#confirmPassword").val("");
			});
		},

		GetEvent: function () {
			return {
				Update: function() {
					$("#updateProfile").click(function (e) {
						e.preventDefault();
						if (profile.IsValid()) {
							profile.UpdateProfile();
						}
					});
				},

				ValidateProfile: function () {
					$("#profileForm").validate({
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
							user: "required",
							name: "required",
							phone: {
								required: true,
								pattern: /^\d+$/
							},
							email: {
								required: true,
								pattern: base.getRegularExpressionEmail
							},
							address: "required",
							zipcode: "required",
							confirmPassword: {
								equalTo : "#newPassword"
							}
						},

						messages: {
							user: "Ingrese el nombre de usuario",
							name: "Ingrese el nombre completo",
							phone: {
								required: "Ingrese un numero de teléfono",
								pattern: "El numero de teléfono solo debe contener numeros"
							},
							email: {
								required: "Ingrese un email",
								pattern: "Formato de correo invalido"
							},
							address: "Ingrese la dirección completa",
							zipcode: "Ingrese el codigo postal",
							confirmPassword: "Las contarseña deben ser iguales"
						}
					});
				}
			}
		}
	}
}());

$(function () {
	profile.init();
});