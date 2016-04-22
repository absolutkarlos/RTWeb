google.load("visualization", "1", { packages: ["columnchart"] });

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

	return {
		GetRootMetaData: function() { return "/MetaData/Index" },
		GetRootState: function() { return "/MetaData/State" },
		GetRootCity: function() { return "/MetaData/city" },
		GetRootZone: function() { return "/MetaData/Zone" },
		GetRootStepClientCreate: function() { return "/Steps/ClientCreate" },
		GetRootStepContactsCreate: function() { return "/Steps/ContactsCreate" },
		GetRootStepOrderCreate: function() { return "/Steps/OrderCreate" },
		GetRootUpdateOrderPanel: function() { return "/Home/OrderPanel" },
		GetRootInfoOrderPanel: function () { return "/Home/InfoOrderPanel" },
		GetRootInspectionPanel: function () { return "/Home/InspectionPanel" },
		GetRootInstalationPanel: function () { return "/Home/InstalationPanel" },
		GetRootUploadFile: function() { return "/Home/UploadFiles" },
		GetRootClients: function() { return "/MetaData/Clients" },
		GetRootGetClient: function() { return "/MetaData/GetClient" },
		GetRootStepPreFactibilityCreate: function() { return "/Steps/PreFactibilityCreate" },
		GetRootStepInspectionCreate: function() { return "/Steps/InspectionCreate" },
		GetRootStepInstalationCreate: function() { return "/Steps/InstalationCreate" },
		GetRootUpdateStatus: function() { return "/Steps/UpdateOrderStatus" },

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

		init: function() {
			jQuery.fn.exists = function() { return this.length > 0; }
			this.ChangeCountryUserSelected();
			this.LoadCountryUserSelected();
			$("body").animatescroll();
		},

		ApplyNiceScroll: function(contentId) {
			$(contentId).niceScroll();
		},

		RemoveLocalMetaData: function () {
			localStorage.removeItem("MetaData");
		},

		GetCountryAbbrevation: function() {
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
			            //$("#coords").val(marker.position);
			            $("#latitude").val(marker.position.lat);
			            $("#longitude").val(marker.position.lng);
			            infowindow = new window.google.maps.InfoWindow({});
			            var formattedAddress = base.FormaterAddressMaps(results[0]);
			            infowindow.setContent(formattedAddress);
			            infowindow.open(map, marker);
			            $("#sitedetailedadress").val(formattedAddress);
                        //$("coords").val(marker.position)
			        } else {
			            window.alert('No results found');
			        }
			    } else {
			        window.alert('Geocoder failed due to: ' + status);
			    }
			});

		},

		DeleteMarkers: function() {
			for (var i = 0; i < markers.length; i++) {
				markers[i].setMap(map);
			}
			markers = [];
		},

		PlottingComplete: function(theLatLng) {

			path.push(theLatLng);

			//display the final marker
			var marker = new window.google.maps.Marker({
				position: theLatLng,
				map: map
			});
			map.panTo(marker.position);
			markers.push(marker);
			//$("#coords").val(marker.position);
			// Display a polyline of the elevation path.
			var pathOptions = {
				path: path,
				strokeColor: '#0000CC',
				opacity: 0.4,
				map: map
			}
			polyline = new window.google.maps.Polyline(pathOptions);

			//the elevation service request
			var pathRequest = {
				'path': path,
				'samples': 256
			}

			// Initiate the path request.
			elSvc.getElevationAlongPath(pathRequest, base.PlotElevation);
		},

		// Takes an array of ElevationResult objects, draws the path on the map
		// and plots the elevation profile on a Visualization API ColumnChart.
		PlotElevation: function(results, status) {
			if (status === window.google.maps.ElevationStatus.OK) {
				elevations = results;

				// Extract the data from which to populate the chart.
				// Because the samples are equidistant, the 'Sample'
				// column here does double duty as distance along the
				// X axis.
				var data = new window.google.visualization.DataTable();
				data.addColumn('string', 'Sample');
				data.addColumn('number', 'Elevation');
				for (var i = 0; i < results.length; i++) {
					data.addRow(['', elevations[i].elevation]);
				}

				// Draw the chart using the data within its DIV. 
				document.getElementById('elevation_chart').style.display = 'block';
				chart.draw(data, {
					width: 640,
					height: 200,
					legend: 'none',
					titleY: 'Elevation (m)'
				});
			}
		},

		LoadRadioBase: function() {
			//var labels = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
			//var labelIndex = 0;
			var metaData = base.GetLocalMetaData();
			if ((metaData.RadioBase) && (metaData.RadioBase.Data) && (metaData.RadioBase.Data.length > 0)) {
			    $.each(metaData.RadioBase.Data, function (index, item) {
			        var radioBase = new window.google.maps.Circle({
			            strokeColor: '#FFFFFF',
			            strokeOpacity: 0,
			            clickable: false,
			            strokeWeight: 1,
			            fillColor: '#FFFFFF',
			            fillOpacity: 0.1,
			            map: map,
			            center: { lat: parseFloat(item.Latitude), lng: parseFloat(item.Longitude) },
			            radius: (10 * 1000)
			        });
			        //var infowindow = new google.maps.InfoWindow({});
			        var marker = new window.google.maps.Marker({
			            position: { lat: parseFloat(item.Latitude), lng: parseFloat(item.Longitude) },
			            //label: labels[labelIndex++ % labels.length],
			            title: item.Name,
			            map: map
			        });
			        //infowindow.setContent(item.Name);
			        titles.push(item.Name);
			        markers.push(marker);
			    });
			}
			var options_markerclusterer = {
			    zoomOnClick: false
			};

			var markerCluster = new MarkerClusterer(map, markers, options_markerclusterer);

			google.maps.event.addListener(markerCluster, 'clusterclick', function (cluster) {
			    var totalmarkers = cluster.getMarkers();
			    var array = [];
			    var num = 0;
			    for (i = 0; i < totalmarkers.length; i++) {
			        num++;
			        array.push(totalmarkers[i].getTitle() + '<br>');
			    }
			    if (map.getZoom() <= markerCluster.getMaxZoom()) {
			        infoWindow.setContent(totalmarkers.length + " markers<br>" + array);
			        infoWindow.setPosition(cluster.getCenter());
			        infoWindow.open(map);
			    }
			});
		},

		GeoCodeLatLng: function(latlng) {
			var geocoder = new window.google.maps.Geocoder;
			geocoder.geocode({ 'location': latlng }, function(results, status) {
				if (status === window.google.maps.GeocoderStatus.OK) {
					if (results[0]) {
						map.setZoom(12);
						infowindow = new window.google.maps.InfoWindow({});
						if (!!marker) {
						    marker.setpos(null);
						}
                        marker = new window.google.maps.Marker({
							position: latlng,
							map: map
                        });
                        map.panTo(marker.position);
                        //$("#coords").val(marker.position);
						var formattedAddress = base.FormaterAddressMaps(results[0]);
						//infowindow.setContent(formattedAddress);
						//infowindow.open(map, marker);
						$("#sitedetailedadress").val(formattedAddress);
					} else {
						window.alert('No results found');
					}
				} else {
					window.alert('Geocoder failed due to: ' + status);
				}
			});
		},

		HandleGoogelMapError: function(browserHasGeolocation, infoWindow, pos) {
			infoWindow.setPosition(pos);
			infoWindow.setContent(browserHasGeolocation ?
				'Error: El servicio de geolocalización falló.' :
				'Error: Su navegador no soporta geolocalización.');
		},

		InitializeGoogleMap: function() {
			map = new window.google.maps.Map(document.getElementById('googleMap'), {
				zoom: 12,
				mapTypeId: window.google.maps.MapTypeId.HYBRID
			});

			// Create a new chart in the elevation_chart DIV.
			chart = new window.google.visualization.ColumnChart(document.getElementById('elevation_chart'));

			// Create an ElevationService.
			elSvc = new window.google.maps.ElevationService();

			var input = document.getElementById('coords');
			var searchBox = new google.maps.places.SearchBox(input);

			map.addListener('bounds_changed', function () {
			    searchBox.setBounds(map.getBounds());
			});
            
			var markersplaces = [];

			searchBox.addListener('places_changed', function() {
			    var places = searchBox.getPlaces();
			    if (places.length == 0) {
			        return;
			    }
			    markersplaces.forEach(function(marker) {
			        marker.setMap(null);
			    });
			    markersplaces = [];
			    
			    var bounds = new google.maps.LatLngBounds();
			    places.forEach(function(place) {
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
			        
			
			        if (place.geometry.viewport) {
			            // Only geocodes have viewport.
			            bounds.union(place.geometry.viewport);
			        } else {
			            bounds.extend(place.geometry.location);
			        }
			    });
			    map.fitBounds(bounds);
			});



			map.controls[window.google.maps.ControlPosition.TOP_RIGHT].push(FullScreenControl(map, ["Modo pantalla completa"], ["Salir del modo pantalla completa"]));

			// Create a new chart in the elevation_chart DIV.
			chart = new window.google.visualization.ColumnChart(document.getElementById('elevation_chart'));

			// Create an ElevationService.
			elSvc = new window.google.maps.ElevationService();

			//map.controls[google.maps.ControlPosition.TOP_CENTER].push('<input onclick="deleteMarkers();" type=button value="Borrar Marcadores">');

			window.google.maps.event.addListener(map, 'click', function (event) {
			    base.PlotPoints(event.latLng, map);
			});

			window.google.maps.event.addListener(map, 'rightclick', function(event) {
				base.PlottingComplete(event.latLng);
			});

			mouseOverInfowindow = new window.google.maps.InfoWindow({});

			window.google.visualization.events.addListener(chart, 'onmouseover', function(e) {
				var contentStr;
				if (mousemarker == null) {
					mousemarker = new window.google.maps.Marker({
						position: elevations[e.row].location,
						map: map,
						icon: "http://maps.google.com/mapfiles/ms/icons/green-dot.png"
					});
					contentStr = "elevation=" + elevations[e.row].elevation + "<br>location=" + elevations[e.row].location.toUrlValue(6);
					mousemarker.contentStr = contentStr;
					window.google.maps.event.addListener(mousemarker, 'click', function () {
					    mmInfowindowOpen = true;
						mouseOverInfowindow.setContent(this.contentStr);
						mouseOverInfowindow.open(map, mousemarker);
					});
				} else {
					contentStr = "elevation=" + elevations[e.row].elevation + "<br>location=" + elevations[e.row].location.toUrlValue(6);
					mousemarker.contentStr = contentStr;
					mouseOverInfowindow.setContent(contentStr);
					mousemarker.setPosition(elevations[e.row].location);
					// if (mm_infowindow_open) infowindow.open(map,mousemarker);
				}
			});

			if (navigator.geolocation) {
				navigator.geolocation.getCurrentPosition(function(position) {
					var pos = {
						lat: position.coords.latitude,
						lng: position.coords.longitude
					};
                  
					$("#longitude").val(pos.lng);
					$("#latitude").val(pos.lat);

					base.GeoCodeLatLng(pos);

					//infoWindow.setPosition(pos);
					//infoWindow.setContent('Esta es tu ubicación actual.');
					map.setCenter(new window.google.maps.LatLng(pos.lat, pos.lng));
				}, function () {
					infoWindow = new window.google.maps.InfoWindow({ map: map });
					HandleGoogelMapError(true, infoWindow, map.getCenter());
				});
			} else {
				// Browser doesn't support Geolocation
				infoWindow = new window.google.maps.InfoWindow({ map: map });
				HandleGoogelMapError(false, infoWindow, map.getCenter());
			}
		},

		FormaterAddressMaps: function(address) {
			var formattedAddress = "";
			var length = address.address_components.length;
			if (length > 0) {
				var addressComponentsrRverse = address.address_components.reverse();
				$.each(addressComponentsrRverse, function(index, item) {
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


		ValidateHasError: function(data, callback) {
			var valid = (data.ErrorMessage !== null);
			if (valid) {
				if (data.Status === 401) {
					alert("Su sesion a expirado");
				} else {
					callback();
				}
			}
			return valid;
		},

		ErrorAjax: function(data) {
			if (data) {

			}
		},

		RefreshMap: function() {
			window.google.maps.event.trigger(map, 'resize');
		},

		GetLocalMetaData: function() {
			if (localStorage.getItem("MetaData")) {
				return JSON.parse(localStorage.getItem("MetaData"));
			}
			return null;
		},

		LoadDropDownList: function(selector, data) {
			$.each(data, function() {
				$(selector).append($("<option />").val(this.Id).text(this.Name));
			});
		},

		ResetDropDownList: function(selector) {
			$(selector + ' option[value=""]').prop("selected", true);
			$(selector).select2("");
		},

		ClearDropDownList: function(selector) {
			var option = $(selector + ' option[value=""]');
			$(selector).empty();
			$(selector).append(option);
			$(selector + ' option[value=""]').prop("selected", true);
			$(selector).select2("");
		},

		getLanguageCookie: function() {
			return self.readCookie(self.getEducaStoreCookie);
		},

		checkIfCookieExist: function() {

			var self = this;

			if (self.readCookie(self.getEducaStoreCookie) == null)
				self.createCookie(self.getEducaStoreCookie, self.getEducaStoreDefaultLang, 365);
		},

		isBrowserMobile: function() {

			var returning = false;

			if ((this.isMobile.Android()) || (this.isMobile.iOS()) || (this.isMobile.BlackBerry()) || (this.isMobile.Windows())) {
				returning = true;
			}

			return returning;
		},


		checkIfIE: function() {
			var ua = window.navigator.userAgent;
			var msie = ua.indexOf("MSIE ");

			if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))
				return true;
			else
				return false;
		},


		createCookie: function(name, value, days) {
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


		readCookie: function(name) {
			var nameEq = name + "=";
			var ca = document.cookie.split(';');
			for (var i = 0; i < ca.length; i++) {
				var c = ca[i];
				while (c.charAt(0) === ' ') c = c.substring(1, c.length);
				if (c.indexOf(nameEq) === 0) return c.substring(nameEq.length, c.length);
			}
			return null;
		},


		eraseCookie: function(name) {
			this.createCookie(name, "", -1);
		},

		/*Format strings with arguments*/
		format: function(str, arguments1) {
			for (var i = 0; i < arguments1.length; i++) {
				var reg = new RegExp("\\{" + i + "\\}", "gm");
				str = str.replace(reg, arguments1[i]);
			}
			return str;
		},

		/*Gets all query string and returns them as an array*/
		getUrlValues: function() {
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
			Android: function() {
				return /Android/i.test(navigator.userAgent);
			},
			BlackBerry: function() {
				return /BlackBerry/i.test(navigator.userAgent);
			},
			iOS: function() {
				return /iPhone|iPad|iPod/i.test(navigator.userAgent);
			},
			Windows: function() {
				return /IEMobile/i.test(navigator.userAgent);
			}
		},


		/* Method to redirect to specific URL*/
		redirectURL: function(url) {
			window.location.href = url;
		},

		ChangeCountryUserSelected: function() {
			$(".optionCountry").click(function() {
				$("#countrySelected").empty().append($(this).html());
				localStorage.setItem("countrySelected", $("#countrySelected").find("img").data("country"));
			});
		},

		GetCountryUserSelected: function() {
			return $("#countrySelected").find("img").data("country");
		},

		LoadCountryUserSelected: function() {
			if(localStorage.getItem("countrySelected")) {
				$.each($(".optionCountry"), function(index, item) {
					if ($(item).find("img").data("country") === localStorage.getItem("countrySelected")) {
						$("#countrySelected").empty().append($(item).html());
					}
				});
			}
		}
}
}());

$(function () {
	base.init();
});