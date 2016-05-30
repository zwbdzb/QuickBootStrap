// JavaScript source code

function CustomOverlay(point, text, option) {
    this._point = point;
    this._text = text;
    this._option = option;
}

CustomOverlay.prototype = new BMap.Overlay();

CustomOverlay.prototype.initialize = function (map) {
    this._map = map;
    var div = this._div = document.createElement("div");

    div.style.position = 'absolute';
    div.style.zIndex = BMap.Overlay.getZIndex(this._point.lat);
    div.style.width = '24px';
    div.style.height = '24px';
    div.style.lineHeight = '24px';
    div.style.whiteSpace = "nowrap";
    div.style.fontSize = '18px';
    div.className = 'glyphicon glyphicon-facetime-video';       // 'glyphicon glyphicon-map-marker';
    div.style.color = 'purple';
    var content = this._span = document.createElement("span");
    content.className = 'label label-default';
    div.appendChild(content);

    var that = this;
    var $map = $(this._map.Ua);

    div.onmouseenter = function () {
        content.className = 'label label-default';
        this.getElementsByTagName("span")[0].innerHTML = that._text;
    }

    div.onmouseleave = function () {
        content.className = '';
        this.getElementsByTagName("span")[0].innerHTML = '';
    }

    div.ondragstart = function (event, ui) {
        map.disableDragging();
        var pointStart = false;
        var x = event.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
        var y = event.clientY + document.body.scrollTop + document.documentElement.scrollTop;
        x = x - $map.offset().left;
        y = y - $map.offset().top;
        pointStart = map.pixelToPoint(new BMap.Pixel(x, y));
        that._point = pointStart;               // 这里不要使用 this._point,会给div DOM无故添加 _point 属性，要使用that
        console.log('dragstart:' + pointStart.lng + '  ' + pointStart.lat);
    }

    div.ondragstop = function (event, ui) {
        var pointEnd = false;
        var x = ui.offset.left + $(this).width() / 2;
        var y = ui.offset.top + $(this).height() / 2;

        x = x - $map.offset().left;
        y = y - $map.offset().top;
        pointEnd = map.pixelToPoint(new BMap.Pixel(x, y));
        console.log('dragstop:' + pointEnd.lng + '  ' + pointEnd.lat);
        that._point = pointEnd;
        map.enableDragging();
    }

    $(div).draggable({ scroll: true, opacity: 0.35 });
    map.getPanes().markerPane.appendChild(div);
    return div;
}

CustomOverlay.prototype.draw = function () {
    var map = this._map;
    var pixel = map.pointToOverlayPixel(this._point);
    this._div.style.left = pixel.x - $(this._div).width() / 2 + 'px';           // 这里有一个坑，必须携带px
    this._div.style.top = pixel.y - $(this._div).height() / 2 + 'px';
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

// 不能定义 overlay原型对象的同名remove的方法，会覆盖原对象的remove，形成死循环
//CustomOverlay.prototype.remove = function () {
//    window.map.removeOverlay(this);
//    return false;
//}

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

// $('glyphicon-facetime-video').draggable();







