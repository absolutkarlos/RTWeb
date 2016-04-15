var client = (function () {
	return {
		init: function () {
			base.InitializeGoogleMap();
		},

		ResetForm: function () {
			this.ClearClientId();
			base.ResetDropDownList("#country");
			base.ResetDropDownList("#clienttype");
			base.ClearDropDownList("#state");
			base.ClearDropDownList("#city");
			base.ClearDropDownList("#zone");
			$("#fullname").val("");
			$("#ruc").val("");
			$("#detailedadress").val("");
			$("#businessname").val("");
			$(".edit").show();
			$(".readOnly").hide();
			$(".existingClient").hide();
		},

		ShowContentReadOnly: function() {
			$("#labelClientType").text("(" + $("#clienttype").select2('data')[0].text + ")");
			$("#labelUbicacion").text($("#zone").select2('data')[0].text + ", " + $("#state").select2('data')[0].text + ", " + $("#country").select2('data')[0].text);
			$("#labelFullName").text($("#fullname").val());
			$("#labelBusinessName").text($("#businessname").val());
			$("#labelRuc").text($("#ruc").val());
			$("#labelDetailedAdress").text($("#detailedadress").val());
			$(".edit").hide();
			$(".readOnly").show();
		},

		GetClient: function () {
			var client = {
				"Client.BusinessName": $("#businessname").val(),
				"Client.LegalName": $("#fullname").val(),
				"Client.Ruc": $("#ruc").val(),
				"Client.ClientType.Id": $("#clienttype").val(),
				"Client.AddressRef": $("#detailedadress").val(),
				"Client.Country.Id": $("#country").val(),
				"Client.State.Id": $("#state").val(),
				"Client.City.Id": $("#city").val(),
				"Client.Zone.Id": $("#zone").val()
			}
			return client;
		},

		ClearClientId: function () {
			localStorage.removeItem("ClientId");
		},

		LoadClientId: function (id) {
			if (localStorage.ClientId != null) {
				localStorage.ClientId = id;
			} else {
				localStorage.setItem("ClientId", id);
			}
		},

		GetClientId: function() {
			if (localStorage.ClientId != null) {
				return parseInt(localStorage.ClientId.toString());
			}
			return 0;
		},

		ClientCreate: function () {
			return baseClient.ClientCreate(this.GetClient()).success(function(response) {
				var valid = base.ValidateHasError(response, function() {
					alert("error creando el cliente");
				});

				if (!valid) {
					client.LoadClientId(response.Data.toString());
				}

				return valid;
			});
		},

		GetEvent: function () {
			return {

			}
		}
	}
}());

$(function () {
	client.init();
});