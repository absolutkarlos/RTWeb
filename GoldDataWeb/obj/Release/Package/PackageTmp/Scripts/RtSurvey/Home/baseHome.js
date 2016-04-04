var baseHome = (function () {
	return {
		init: function () {

		},

		GetExistingClient: function (clientId) {
			return $.ajax({
				method: "GET",
				dataType: "json",
				data: {
					"clientId": clientId
				},
				url: base.GetRootGetClient(),
				error: base.ErrorAjax
			});
		},

		GetAllClients: function () {
			return $.ajax({
				method: "GET",
				dataType: "json",
				url: base.GetRootClients(),
				error: base.ErrorAjax
			});
		},

		LoadInfoOrderPanel: function (orderId) {
			return $.ajax({
				method: "GET",
				dataType: "html",
				data: {
					"orderId": orderId
				},
				url: base.GetRootInfoOrderPanel(),
				error: base.ErrorAjax
			});
		},

		UpdatePanelOrder: function () {
			return $.ajax({
				method: "GET",
				dataType: "html",
				url: base.GetRootUpdateOrderPanel(),
				beforeSend: function () {
					home.GetEvent().IconRefreshAnimated("#refreshOrders");
				},
				error: base.ErrorAjax
			});
		},

		GetAllMetaData: function () {
			return $.ajax({
				method: "GET",
				dataType: "json",
				url: base.GetRootMetaData(),
				error: base.ErrorAjax
			});
		},

		GetStatesByCountry: function (countryId) {
			return $.ajax({
				method: "GET",
				data: {
					"countryId": countryId
				},
				dataType: "json",
				url: base.GetRootState(),
				error: base.ErrorAjax
			});
		},

		GetCitiesByState: function (stateId) {
			return $.ajax({
				method: "GET",
				data: {
					"stateId": stateId
				},
				dataType: "json",
				url: base.GetRootCity(),
				error: base.ErrorAjax
			});
		},

		GetZonesByCity: function (cityId) {
			return $.ajax({
				method: "Get",
				data: {
					"cityId": cityId
				},
				dataType: "json",
				url: base.GetRootZone(),
				error: base.ErrorAjax
			});
		}
	}
}());

$(function () {
	baseHome.init();
});