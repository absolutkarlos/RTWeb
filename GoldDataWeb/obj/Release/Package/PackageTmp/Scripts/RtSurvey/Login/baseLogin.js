var baseLogin = (function () {
	return {
		init: function () {
			$("#rememberme").bootstrapSwitch("size", "mini");
		}
	}
}());

$(function () {
	baseLogin.init();
});