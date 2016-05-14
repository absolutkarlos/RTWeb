var map;

var base = (function () {
	var elSvc;
	var chart;
	var polyline;
	var path = new Array();
	var mousemarker = null;
	var elevations = null;
	var mouseOverInfowindow = null;
	var mmInfowindowOpen;
	var marker;
	var markers = [];
	var countryAbbrevation = "";
	var infowindow;
	var titles = [];
	var radiobasespoints = [];
	var markerCluster;
	var formattedAddress;
	var infoWindowRB;
	var infoWindowRD;
	var intervalCountdown;
	var timeOutToken;
	var currentSlide;
	var rand;

	return {
		GetRootLogOff: function () { return "/Login/LogOff" },
		GetRootRefreshToken: function () { return "/Login/RefreshToken" },
		GetRootMetaData: function () { return "/MetaData/Index" },
		GetRootState: function () { return "/MetaData/State" },
		GetRootCity: function () { return "/MetaData/city" },
		GetRootZone: function () { return "/MetaData/Zone" },
		GetRootStepClientCreate: function () { return "/Steps/ClientCreate" },
		GetRootStepContactsCreate: function () { return "/Steps/ContactsCreate" },
		GetRootStepOrderCreate: function () { return "/Steps/OrderCreate" },
		GetRootUpdateOrderPanel: function () { return "/Home/OrderPanel" },
		GetRootInfoOrderPanel: function () { return "/Home/InfoOrderPanel" },
		GetRootInspectionPanel: function () { return "/Home/InspectionPanel" },
		GetRootInstalationPanel: function () { return "/Home/InstalationPanel" },
		GetRootUploadFile: function () { return "/Home/UploadFiles" },
		GetRootClients: function () { return "/MetaData/Clients" },
		GetRootGetClient: function () { return "/MetaData/GetClient" },
		GetRootStepPreFactibilityCreate: function () { return "/Steps/PreFactibilityCreate" },
		GetRootStepInspectionCreate: function () { return "/Steps/InspectionCreate" },
		GetRootStepInstalationCreate: function () { return "/Steps/InstalationCreate" },
		GetRootUpdateStatus: function () { return "/Steps/UpdateOrderStatus" },
		GetRootExistingClientValidate: function () { return "/Home/ExistingClientValidate" },
		GetRootUpdateProfile: function () { return "/Home/UpdateProfile" },

		defaultAjaxTimeout: 5000,

		isWaiting: false,

		getEducaStoreDefaultLang: "EN",

		getNotificationTypeInfo: "info",

		getNotificationTypeSuccess: "success",

		getNotificationTypeWarning: "warning",

		getDefaultNotificationMessageDuration: 5000,

		getRegularExpressionEmail: "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*",

		getRegularExpressionUsername: "^[a-zA-Z0-9]+$",

		getRegularExpressionPassword: "^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\!\#\@\_\.\*\$]).{8,15}$",

		getRegularExpressionName: "^[a-zA-ZàáâäãåąčćęèéêëėįìíîïńòóôöõùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŃÒÓÔÖÕÙÚÛÜŲŪŸÝŻŹÑÇČŠŽ ,.'´-]{2,40}$",

		getRegularExpressionPhone: "/\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\\2([0-9]{4})/",

		init: function () {
			jQuery.fn.exists = function () { return this.length > 0; }
			this.ChangeCountryUserSelected();
			this.LoadCountryUserSelected();
			$("body").animatescroll();
			base.ToolBarEvent();
		},

		InitializeCarousel: function () {
			$('.carousel').carousel({
				interval: 1200000
			});
			currentSlide = Math.floor((Math.random() * $('.item').length));
			rand = currentSlide;
			$('#myCarousel').carousel(currentSlide);
			$('#myCarousel').fadeIn(1000);
			setInterval(function () {
				while (rand == currentSlide) {
					rand = Math.floor((Math.random() * $('.item').length));
				}
				currentSlide = rand;
				$('#myCarousel').carousel(rand);
			}, 1199999);
		},

		VaidateDecimalNumber: function (number) {
			var reg = /^-?\d+\.?\d*$/;
			return reg.test(number.trim());
		},

		ApplyNiceScroll: function (contentId) {
			$(contentId).niceScroll();
		},

		RemoveLocalMetaData: function () {
			localStorage.removeItem("MetaData");
		},

		GetCountryAbbrevation: function () {
			return countryAbbrevation;
		},

		PlotPoints: function (theLatLng, map) {
			path.push(theLatLng);
			if (!!marker) {
				marker.setMap(null);
			}
			marker = new window.google.maps.Marker({
				position: theLatLng,
				map: map
			});
			map.panTo(marker.position);

			var geocoder = new window.google.maps.Geocoder;
			geocoder.geocode({ 'location': marker.position }, function (results, status) {
				if (status === window.google.maps.GeocoderStatus.OK) {
					if (results[0]) {
						$("#latitude").val(marker.position.lat);
						$("#longitude").val(marker.position.lng);
						formattedAddress = base.FormaterAddressMaps(results[0]);
						$("#sitedetailedadress").val(formattedAddress);
					} else {
						window.alert('No results found');
					}
				} else {
					window.alert('Geocoder failed due to: ' + status);
				}
			});

		},

		DeleteMarkers: function () {
			for (var i = 0; i < markers.length; i++) {
				markers[i].setMap(map);
			}
			markers = [];
		},

		LoadRadioBase: function () {
			var metaData = base.GetLocalMetaData();
			if ((metaData.RadioBase) && (metaData.RadioBase.Data) && (metaData.RadioBase.Data.length > 0)) {
				markers = [];
				$.each(metaData.RadioBase.Data, function (index, item) {
					var radioBase = new window.google.maps.Circle({
						strokeColor: '#0000FF',
						strokeOpacity: 0.9,
						clickable: false,
						strokeWeight: 1,
						fillColor: 'black',
						fillOpacity: 0,
						map: map,
						center: { lat: parseFloat(item.Latitude), lng: parseFloat(item.Longitude) },
						radius: (parseInt(item.Diameter) * 1000)
					});

					var infowindowpoint = new window.google.maps.InfoWindow({
						position: { lat: parseFloat(item.Latitude), lng: parseFloat(item.Longitude) },
						content: item.Name,
						map: map
					});

					var point = { lat: parseFloat(item.Latitude), lng: parseFloat(item.Longitude) };

					titles.push(item.Name);
					markers.push(infowindowpoint);
				});
			}
			var options_markerclusterer = {
				maxzoom: 15,
				zoomOnClick: false,
				gridSize: 20
			};

			markerCluster = new MarkerClusterer(map, markers, options_markerclusterer);
			infoWindowRD = new window.google.maps.InfoWindow({});

			google.maps.event.addListener(markerCluster, 'clusterclick', function (cluster, event) {
				var clustertot = cluster.getMarkers();
				var array = "";

				for (var i = 0; i < clustertot.length; i++) {
					array += clustertot[i].content + '<br>';
				}

				infoWindowRD.setContent(array);
				infoWindowRD.setPosition(cluster.getCenter());
				infoWindowRD.open(map);
			});
			$("#googleMap").show();
			$("#loadingMap").hide();
			base.RefreshMap();
		},

		GeoCodeLatLng: function (latlng) {
			var geocoder = new window.google.maps.Geocoder;
			geocoder.geocode({ 'location': latlng }, function (results, status) {
				if (status === window.google.maps.GeocoderStatus.OK) {
					if (results[0]) {
						var latitude = results[0].geometry.location.lat();
						var longitude = results[0].geometry.location.lng();

						map.setZoom(15);
						infowindow = new window.google.maps.InfoWindow({});
						if (!!marker) {
							marker.setPosition(null);
						}
						marker = new window.google.maps.Marker({
							position: latlng,
							map: map
						});
						map.panTo(marker.position);
						var formattedAddress = base.FormaterAddressMaps(results[0]);
						infowindow.setContent(formattedAddress);
						infowindow.open(map, marker);
						$("#sitedetailedadress").val(formattedAddress);
						$("#longitude").val(longitude);
						$("#latitude").val(latitude);
						map.setCenter(new window.google.maps.LatLng(latitude, longitude));
					} else {
						window.alert('Resultado no encontrado');
					}
				} else {
					//window.alert('Geocoder failed due to: ' + status);
					window.alert('La busqueda ha fallado.');
				}
			});
		},

		HandleGoogelMapError: function (browserHasGeolocation, infoWindow, pos) {
			infoWindow.setPosition(pos);
			infoWindow.setContent(browserHasGeolocation ?
				'Error: El servicio de geolocalización falló.' :
				'Error: Su navegador no soporta geolocalización.');
		},

		SearchCoordinate: function () {
			var coords = $("#coords").val().split(',');
			if (coords.length <= 1)
				coords = $("#coords").val().split(' ');

			if (coords.length > 1) {
				if (base.VaidateDecimalNumber(coords[0]) && base.VaidateDecimalNumber(coords[1])) {
					var lat = parseFloat(coords[0]);
					var lng = parseFloat(coords[1]);
					base.GeoCodeLatLng({ lat: lat, lng: lng });
					return true;
				}
				return false;
			}
			return false;
		},

		InitializeGoogleMap: function () {
			map = new window.google.maps.Map(document.getElementById('googleMap'), {
				zoom: 12,
				mapTypeId: window.google.maps.MapTypeId.HYBRID
			});

			var input = document.getElementById('coords');
			var searchBox = new google.maps.places.SearchBox(input);

			map.addListener('bounds_changed', function () {
				searchBox.setBounds(map.getBounds());
			});

			var markersplaces = [];

			searchBox.addListener('places_changed', function () {
				if (!base.SearchCoordinate()) {
					var places = searchBox.getPlaces();

					if (places.length == 0) {
						return;
					}

					markersplaces.forEach(function (marker) {
						marker.setMap(null);
					});
					markersplaces = [];

					var bounds = new google.maps.LatLngBounds();
					places.forEach(function (place) {
						var icon = {
							url: place.icon,
							size: new google.maps.Size(71, 71),
							origin: new google.maps.Point(0, 0),
							anchor: new google.maps.Point(17, 34),
							scaledSize: new google.maps.Size(25, 25)
						};
						if (!!marker) {
							marker.setMap(null);
						}

						marker = new window.google.maps.Marker({
							position: place.geometry.location,
							map: map,
							title: place.name
						});

						base.GeoCodeLatLng(marker.position);

						if (place.geometry.viewport) {
							// Only geocodes have viewport.
							bounds.union(place.geometry.viewport);
						} else {
							bounds.extend(place.geometry.location);
						}
					});
					map.fitBounds(bounds);
				} else {
					marker.setMap(null);
				}
			});

			map.controls[window.google.maps.ControlPosition.TOP_RIGHT].push(FullScreenControl(map, ["Modo pantalla completa"], ["Salir del modo pantalla completa"]));

			window.google.maps.event.addListener(map, 'click', function (event) {
				base.PlotPoints(event.latLng, map);
			});

			window.google.maps.event.addListener(map, 'zoom_changed', function () {
				if (infoWindowRD !== undefined)
					infoWindowRD.close();
			});

			var geocoder = new window.google.maps.Geocoder;
			geocoder.geocode({ 'location': { lat: parseFloat("8.4266475"), lng: parseFloat("-81.2265862") } }, function (results, status) {
				if (status === window.google.maps.GeocoderStatus.OK) {
					if (results[0]) {
						var latitude = results[0].geometry.location.lat();
						var longitude = results[0].geometry.location.lng();

						$("#coords").val("");
						$("#sitedetailedadress").val("");
						$("#longitude").val("");
						$("#latitude").val("");
						map.setZoom(4);
						map.setCenter(new window.google.maps.LatLng(latitude, longitude));
					}
				}
			});

			//if (navigator.geolocation) {
			//	navigator.geolocation.getCurrentPosition(function (position) {
			//		var pos = {
			//			lat: position.coords.latitude,
			//			lng: position.coords.longitude
			//		};

			//		$("#longitude").val(pos.lng);
			//		$("#latitude").val(pos.lat);

			//		base.GeoCodeLatLng(pos);
			//	}, function () {
			//		base.GeoCodeLatLng({ lat: parseFloat("6.8678296"), lng: parseFloat("-83.0290863") });
			//	});
			//} else {
			//	// Browser doesn't support Geolocation
			//	infoWindow = new window.google.maps.InfoWindow({ map: map });
			//	base.HandleGoogelMapError(false, infoWindow, map.getCenter());
			//}

			base.LoadRadioBase();
		},

		FormaterAddressMaps: function (address) {
			var formattedAddress = "";
			var length = address.address_components.length;
			if (length > 0) {
				var addressComponentsrRverse = address.address_components.reverse();
				$.each(addressComponentsrRverse, function (index, item) {
					if (item.types[0] === "country") {
						countryAbbrevation = item.short_name;
					}
					formattedAddress += item.long_name;
					if (index < (length - 1)) {
						formattedAddress += ", ";
					}
				});
			} else {
				formattedAddress = address.formatted_address;
			}
			return formattedAddress;
		},

		RefreshToken: function () {
			return $.ajax({
				method: "POST",
				url: base.GetRootRefreshToken(),
				error: base.ErrorAjax
			});
		},

		ValidateExpireToken: function () {
			base.RefreshToken();
			clearInterval(intervalCountdown);
			clearTimeout(timeOutToken);
			var data = JSON.parse(base.readCookie("RefreshToken"));
			timeOutToken = setTimeout(function () {
				base.ShowModalSessionExpire();
			}, ((parseInt(data.ExpiresIn) * 1000) - 40000));
		},

		Countdown: function () {
			var seg = parseInt($("#timeSession").text());
			if (seg > 0) {
				$("#timeSession").text(seg - 1);
			} else {
				clearInterval(intervalCountdown);
				clearTimeout(timeOutToken);
				$('#confirm').modal('hide');
				$.when(baseHome.LogOff()).then(function () {
					$("#timeSession").text(30);
					location.reload();
				});
			}
		},

		ShowModalSessionExpire: function () {
			$('#confirm').modal({ backdrop: 'static', keyboard: false }).one('click', '#buttomModalConfirm', function () {
				clearInterval(intervalCountdown);
				clearTimeout(timeOutToken);
				$("#timeSession").text(30);
				$.when(base.RefreshToken()).then(function (resp) {
					if (resp.UpdateToken) {
						base.ValidateExpireToken();
					} else {
						baseHome.LogOff();
						location.reload();
					}
				});
			});
			$('#buttomModalCancel').on('click', function (e) {
				$("#timeSession").text(30);
				clearInterval(intervalCountdown);
				clearTimeout(timeOutToken);
				$.when(baseHome.LogOff()).then(function () {
					location.reload();
				});
			});
			intervalCountdown = setInterval(function () {
				base.Countdown();
			}, 1000);
		},

		ValidateHasError: function (data, callback) {
			var valid = (data.ErrorMessage !== null);
			if (valid) {
				if (data.Status === 401) {
					base.ShowModalSessionExpire();
				} else {
					callback();
				}
			}
			return valid;
		},

		ErrorAjax: function (data) {
			if (data) {

			}
		},

		RefreshMap: function () {
			window.google.maps.event.trigger(map, 'resize');
		},

		GetLocalMetaData: function () {
			if (localStorage.getItem("MetaData")) {
				return JSON.parse(localStorage.getItem("MetaData"));
			}
			return null;
		},

		LoadDropDownList: function (selector, data) {
			$.each(data, function () {
				$(selector).append($("<option />").val(this.Id).text(this.Name));
			});
		},

		ResetDropDownList: function (selector) {
			$(selector + ' option[value=""]').prop("selected", true);
			$(selector).select2("");
		},

		ClearDropDownList: function (selector) {
			var option = $(selector + ' option[value=""]');
			$(selector).empty();
			$(selector).append(option);
			$(selector + ' option[value=""]').prop("selected", true);
			$(selector).select2("");
		},

		getLanguageCookie: function () {
			return self.readCookie(self.getEducaStoreCookie);
		},

		checkIfCookieExist: function () {

			var self = this;

			if (self.readCookie(self.getEducaStoreCookie) == null)
				self.createCookie(self.getEducaStoreCookie, self.getEducaStoreDefaultLang, 365);
		},

		isBrowserMobile: function () {

			var returning = false;

			if ((this.isMobile.Android()) || (this.isMobile.iOS()) || (this.isMobile.BlackBerry()) || (this.isMobile.Windows())) {
				returning = true;
			}

			return returning;
		},


		checkIfIE: function () {
			var ua = window.navigator.userAgent;
			var msie = ua.indexOf("MSIE ");

			if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))
				return true;
			else
				return false;
		},


		createCookie: function (name, value, days) {
			var expires;
			if (days) {
				var date = new Date();
				date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
				expires = "; expires=" + date.toGMTString();
			} else {
				expires = "";
			}
			document.cookie = name + "=" + value + expires + "; path=/";
		},


		readCookie: function (name) {
			var nameEq = name + "=";
			var ca = document.cookie.split(';');
			for (var i = 0; i < ca.length; i++) {
				var c = ca[i];
				while (c.charAt(0) === ' ') c = c.substring(1, c.length);
				if (c.indexOf(nameEq) === 0) return c.substring(nameEq.length, c.length);
			}
			return null;
		},


		eraseCookie: function (name) {
			this.createCookie(name, "", -1);
		},

		/*Format strings with arguments*/
		format: function (str, arguments1) {
			for (var i = 0; i < arguments1.length; i++) {
				var reg = new RegExp("\\{" + i + "\\}", "gm");
				str = str.replace(reg, arguments1[i]);
			}
			return str;
		},

		/*Gets all query string and returns them as an array*/
		getUrlValues: function () {
			var vars = [], hash;
			var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
			for (var i = 0; i < hashes.length; i++) {
				hash = hashes[i].split('=');
				vars.push(hash[0]);
				vars[hash[0]] = hash[1];
			}
			return vars;
		},

		/* Set of functions to validate if is an specific mobile OS */
		isMobile: {
			Android: function () {
				return /Android/i.test(navigator.userAgent);
			},
			BlackBerry: function () {
				return /BlackBerry/i.test(navigator.userAgent);
			},
			iOS: function () {
				return /iPhone|iPad|iPod/i.test(navigator.userAgent);
			},
			Windows: function () {
				return /IEMobile/i.test(navigator.userAgent);
			}
		},


		/* Method to redirect to specific URL*/
		redirectURL: function (url) {
			window.location.href = url;
		},

		ChangeCountryUserSelected: function () {
			$(".optionCountry").click(function () {
				$("#countrySelected").empty().append($(this).html());
				localStorage.setItem("countrySelected", $("#countrySelected").find("img").data("country"));
			});
		},

		GetCountryUserSelected: function () {
			return $("#countrySelected").find("img").data("country");
		},

		LoadCountryUserSelected: function () {
			if (localStorage.getItem("countrySelected")) {
				$.each($(".optionCountry"), function (index, item) {
					if ($(item).find("img").data("country") === localStorage.getItem("countrySelected")) {
						$("#countrySelected").empty().append($(item).html());
					}
				});
			}
		},

		ToolBarEvent: function () {
			$(".toolbar").click(function () {
				$.each($(".toolbar"), function (index, item) {
					$(item).removeClass("toolbaractive");
				});
				$(this).addClass("toolbaractive");
			});
		}
	}
}());

$(function () {
	base.init();
});