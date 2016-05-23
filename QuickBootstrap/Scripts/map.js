window.map = false;
window.mode = 0;                       // play mode or mao mode
window.flag = 0;                       // edit 1 ，load 0
window.markers = [];
window.markerClusterer = false;

// #region custom control

function ZoomControl() {
	this.defaultAnchor = BMAP_ANCHOR_TOP_RIGHT;
	this.defaultOffset = new BMap.Size(10, 40);
}

if (window.BMap && window.BMap.Control) {
	ZoomControl.prototype = new BMap.Control();
}

ZoomControl.prototype.initialize = function (map) {
	var div = document.createElement("div");
	div.appendChild(document.createTextNode("编辑标记"));
	div.style.cursor = "pointer";
	div.style.border = "1px solid #A6C2DE";
	div.style.backgroundColor = "#8BA4DC";
	div.style.color = "white";
	div.style.padding = "5px";
	div.onclick = function (e) {
		if (flag == 0) {
			flag = 1;
			div.childNodes[0].textContent = "退出编辑";
		} else {
			flag = 0;
			div.childNodes[0].textContent = "编辑标记";
		}
	}
	// add dom for the map
	map.getContainer().appendChild(div);
	// 将DOM元素返回
	return div;
}

//#endregion

// #region map init 

// init map
function initMap() {
	createMap();						
	setMapEvent();						
	addMapControl();					
}

function createMap() {
	map = new BMap.Map("map");
	map.centerAndZoom(new BMap.Point(106.267089, 38.519694), 5);  

	// position map center by the ip
	/*var local = new BMap.LocalCity();
	local.get(function (LocalCityResult) {
		map.centerAndZoom(LocalCityResult.center,13);
	});*/

	//map.enableScrollWheelZoom(true);
	//BMap.Map.prototype.GetOverlay = function (tmp) {
	//	var overlays = this.getOverlays();
	//	for (x in overlays) {
	//		if (!!overlays[x]._option && overlays[x]._option.id == tmp)
	//			return overlays[x];
	//	}
	//};
}

function setMapEvent() {
	map.enableDragging();				//启用地图拖拽事件，默认启用(可不写)
	map.enableScrollWheelZoom();		//启用地图滚轮放大缩小
	map.enableDoubleClickZoom();		//启用鼠标双击放大，默认启用(可不写)
	map.enableKeyboard();				//启用键盘上下左右键移动地图
}

function addMapControl() {
	map.addControl(new BMap.MapTypeControl());
	var ctrl_nav = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_LARGE });
	map.addControl(ctrl_nav);
	var ctrl_ove = new BMap.OverviewMapControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, isOpen: 1 });
	map.addControl(ctrl_ove);
	var ctrl_sca = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT });
	map.addControl(ctrl_sca);

	var myZoomCtrl = new ZoomControl();
	map.addControl(myZoomCtrl);

	markerClusterer = new BMapLib.MarkerClusterer(map, { "maxZoom": "9" });
}

// #endregion 


//  add marker
function createMarker(pt, obj) {
	var dataJson = analyzeTreeData(obj);
	obj.li_attr.lng = pt.lng;
	obj.li_attr.lat = pt.lat;
	try {
		$.ajax({
			url: "/Device/DeviceMarker?id=" + dataJson.id.substring(0, 36) + "&&lat=" + pt.lat + "&&lng=" + pt.lng,
			type: "POST",
			dataType: "json",
			success: function (data) {
				if (!data.Result) {
					Alert.TipInside(data.Msg);
					return;
				}
			}
		});
		var marker = loadMarker(pt, obj);
		markerClusterer.addMarker(marker);
		return true;
	} catch (err) {
		Alert.TipInside(err);
		return false;
	}
}

function loadMarker(pt, obj) {
	var dataJson = analyzeTreeData(obj);
	var myCompOverlay = new ComplexCustomOverlay(pt, dataJson);
	map.addOverlay(myCompOverlay);
	markers.push(myCompOverlay);
	return myCompOverlay;
}

// 解析树节点事件数据data
function analyzeTreeData(obj) {
	var devData = obj.li_attr;
	return devData;
}