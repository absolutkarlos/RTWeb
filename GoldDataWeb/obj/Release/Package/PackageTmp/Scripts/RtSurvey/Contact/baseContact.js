var baseContact = (function () {
	return {
		init: function () {

		},

		ContactsCreate: function (data) {
			return $.ajax({
				method: "POST",
				async: false,
				data: data,
				processData: false,
				contentType: "application/json",
				dataType: "json",
				url: base.GetRootStepContactsCreate(),
				error: base.ErrorAjax
			});
		}
	}
}());

$(function () {
	baseContact.init();
});