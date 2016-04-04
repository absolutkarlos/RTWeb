var basePreFactibility = (function () {
	return {
		init: function () {

		},

		LineSightCreate: function (orderid, orderStatusId, status, radioBaseId, distance, siteId) {
			return $.ajax({
				method: "POST",
				dataType: "json",
				data: {
					"LineSight.Distance": distance,
					"LineSight.RadioBase.Id": radioBaseId,
					"LineSight.Site.Id": siteId,
					"Order.Id": orderid,
					"Order.OrderStatus.Id": parseInt(orderStatusId) + 1,
					"Order.Status.Id": status
				},
				url: base.GetRootStepPreFactibilityCreate(),
				error: base.ErrorAjax
			});
		}
	}
}());

$(function () {
	basePreFactibility.init();
});