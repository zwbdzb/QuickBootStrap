
/*
 *  Map object extend
 */

function initMap(container) {
    var map = new BMap.Map(container);
    var local = new BMap.LocalCity();
    local.get(function (LocalCityResult) {
        map.centerAndZoom(LocalCityResult.center, 13);
    });

    /* map interaction */
    map.enableScrollWheelZoom(true);   
    map.enableDragging();				
    map.enableScrollWheelZoom();		
    map.enableDoubleClickZoom();		
    map.enableKeyboard();				

    /* init control */
    map.addControl(new BMap.MapTypeControl());
    var ctrl_nav = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_LARGE });
    map.addControl(ctrl_nav);
    var ctrl_ove = new BMap.OverviewMapControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, isOpen: 1 });
    map.addControl(ctrl_ove);
    var ctrl_sca = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT });
    map.addControl(ctrl_sca);

    var myZoomCtrl = new CustomControl();
    map.addControl(myZoomCtrl);

	window.map = map;
	window.map.markerClusterer = new BMapLib.MarkerClusterer(map, { "maxZoom": "9" });
}

BMap.Map.prototype.getOverlay = function (id) {
    var overlays = this.getOverlays();
    for (x in overlays) {
        if (!!overlays[x]._option && overlays[x]._option.id == id)
            return overlays[x];
    }
};



//  静态方法
BMap.Map.markLocation = function (obj) {
    var pt = {
        lng: obj.li_attr.lng,
        lat: obj.li_attr.lat
    }
    /* TODO 后端更新位置*/
    var myOverlay = new CustomOverlay(pt,obj.text, obj);
    map.addOverlay(myOverlay);
    markers.push(myOverlay);
    markerClusterer.addMarker(marker);
}

// #region custom control

function CustomControl() {
    this.defaultAnchor = BMAP_ANCHOR_TOP_RIGHT;
    this.defaultOffset = new BMap.Size(10, 40);
}

CustomControl.prototype = new BMap.Control();

CustomControl.prototype.initialize = function (map) {
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
    map.getContainer().appendChild(div);
    return div;
}

//#endregion