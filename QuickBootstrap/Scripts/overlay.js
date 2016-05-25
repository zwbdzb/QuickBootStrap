// JavaScript source code

// 复杂的自定义覆盖物
function CustomOverlay(point, text, option) {
    this._point = point;
    this._text = text;
    this._option = option;
}

CustomOverlay.prototype = new BMap.Overlay();

CustomOverlay.prototype.initialize = function (map) {
    this._map = map;
    var div = this._div = document.createElement("div");

    div.style.position = "absolute";
    div.style.zIndex = BMap.Overlay.getZIndex(this._point.lat);
    //div.style.backgroundColor = "blue";
    div.style.color = "blue";
    div.style.height = "25px";
    div.style.lineHeight = "24px";
    div.style.whiteSpace = "nowrap";
    div.style.fontSize = "20px";
    div.className = "glyphicon glyphicon-map-marker";

    var content = this._span = document.createElement("span");

   // content.innerHTML = this._text;  // that._option.DeviceName;  
    div.appendChild(content);

    var that = this;

    //// #region 页面箭头部分
    //var arrow = this._arrow = document.createElement("div");
    //arrow.style.background = "url(http://map.baidu.com/fwmap/upload/r/map/fwmap/static/house/images/label.png) no-repeat";
    //arrow.style.position = "absolute";
    //arrow.style.width = "11px";
    //arrow.style.height = "10px";
    //arrow.style.top = "22px";
    //arrow.style.left = "10px";
    //arrow.style.overflow = "hidden";
    //div.appendChild(arrow);
    ////#endregion      

    div.onmouseenter = function () {
        if (flag == 0) {
            //this.style.backgroundColor = "#6BADCA";
            //this.style.borderColor = "#0000ff";
            this.getElementsByTagName("span")[0].innerHTML = that._text;
            //arrow.style.backgroundPosition = "0px -20px";
        }
    }

    div.onmouseleave = function () {
        if (flag == 0) {
            //this.style.backgroundColor = "#EE5D5B";
            //this.style.borderColor = "#BC3B3A";
            this.getElementsByTagName("span")[0].innerHTML = null;         
            // arrow.style.backgroundPosition = "0px 0px";
        }
    }

    div.ondragstart = function (event, ui) {
        map.disableDragging();
        var pointStart = false;
        var x = event.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
        var y = event.clientY + document.body.scrollTop + document.documentElement.scrollTop;
        var position = $('#map').position();
        x = x - position.left;
        y = y - position.top;
        pointStart = map.pixelToPoint(new BMap.Pixel(x, y));
        this._point = pointStart;
        console.log('dragstart:' + pointStart.lng + '  ' + pointStart.lat);
    }

    div.ondragstop = function (event, ui) {
        var pointEnd = false;
        var x = ui.offset.left + 10;
        var y = ui.offset.top + 22 + 10;
        var position = $('#map').position();
        x = x - position.left;
        y = y - position.top;
        pointEnd = map.pixelToPoint(new BMap.Pixel(x, y));
        console.log('dragstop:' + pointEnd.lng + '  ' + pointEnd.lat);
        that._point = pt;
        map.enableDragging();
    }

    $(div).draggable({ scroll: true, opacity: 0.35, disabled: (flag ? false : true) });
    map.getPanes().markerPane.appendChild(div);
    return div;
}

CustomOverlay.prototype.draw = function () {
    var map = this._map;
    var pixel = map.pointToOverlayPixel(this._point);
    this._div.style.left = pixel.x;     // - parseInt(this._arrow.style.left) + "px";
    this._div.style.top = pixel.y;      // - 30 + "px";
}

CustomOverlay.prototype.getPosition = function () {
    return this._point;
}

CustomOverlay.prototype.getMap = function () {
    return this._map;
}

CustomOverlay.prototype.disableDragging = function () {
    $(this._div).draggable("destroy");			
}

CustomOverlay.prototype.enableDragging = function () {
    $(this._div).draggable({ disabled: false });
}

CustomOverlay.prototype.remove = function () {
    map.removeOverlay(this);
}

CustomOverlay.prototype.toggle = function () {
    if (this._div) {
        if (this._div.style.display == "") {
            this.hide();
        }
        else {
            this.show();
        }
    }
}







