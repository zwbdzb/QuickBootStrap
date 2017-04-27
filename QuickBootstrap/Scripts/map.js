
/*
 *  Map object extend
 */

function initMap(container) {
    var map = new BMap.Map(container);
    map.Dom = $("#" + container);
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
    map.addControl(new BMap.MapTypeControl({ anchor: BMAP_ANCHOR_TOP_LEFT } ));
    var ctrl_nav = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, type: BMAP_NAVIGATION_CONTROL_LARGE });
    map.addControl(ctrl_nav);
    var ctrl_ove = new BMap.OverviewMapControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, isOpen: 1 });
    map.addControl(ctrl_ove);
    var ctrl_sca = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT });
    map.addControl(ctrl_sca);

    var myZoomCtrl = new CustomControl();
    map.addControl(myZoomCtrl);
    window.map = map;   
}


BMap.Map.prototype.getOverlay = function (id) {
    var overlays = this.getOverlays();
    for (x in overlays) {
        if (!!overlays[x]._option && overlays[x]._option.id == id)
            return overlays[x];
    }
};

BMap.Map.prototype.markLocation = function (point, obj) {

    /* TODO 后端更新位置*/
    var myOverlay = new CustomOverlay(point, obj.text, obj);
    this.addOverlay(myOverlay);

   // myOverlay.enableDragging();
   // console.log(myOverlay.getJDom().draggable("option"));

    markers.push(myOverlay);
    return true;
}

// #region custom control

function CustomControl() {
    this.defaultAnchor = BMAP_ANCHOR_TOP_RIGHT;
    this.defaultOffset = new BMap.Size(40,10);
}

CustomControl.prototype = new BMap.Control();

CustomControl.prototype.initialize = function (map) {
    var div = document.createElement("div");
    div.className = 'glyphicon glyphicon-star';
    div.appendChild(document.createTextNode("正常模式"));
    div.style.cursor = "pointer";
    div.style.border = '1px solid #A6C2DE';
    div.style.backgroundColor = '#8BA4DC';
    div.style.color = 'white';
    div.style.padding = '5px';
    div.style.borderRadius = '4px';
    div.setAttribute('data-toggle', 'tooltip');     // 改变已有属性的值，或创建新属性
    div.setAttribute('data-placement', 'right');     // 改变已有属性的值，或创建新属性
    div.setAttribute('title', '单击切换模式');     // 改变已有属性的值，或创建新属性
    div.onclick = function (e) {
        if (flag == 0) {
            flag = 1;
            div.childNodes[0].textContent = "编辑模式";
            div.className = 'glyphicon glyphicon-pencil';
            //  改变所有marker的 编辑状态
            var markers = map.getOverlays();
            $.each(markers, function (i,n) {
                if (n instanceof CustomOverlay) {
                    n.enableDragging();
                }
            });
        } else {
            flag = 0;
            div.childNodes[0].textContent = "正常模式";
            div.className = 'glyphicon glyphicon-star';
            // 改变所有marker的编辑状态
            var markers = map.getOverlays();
            $.each(markers, function (i, n) {
                if (n instanceof CustomOverlay) {
                    n.disableDragging();
                }
            });
        }
    }
    map.getContainer().appendChild(div);
    return div;
}

//#endregion