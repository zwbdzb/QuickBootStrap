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
    div.appendChild(content);
    
    var that = this;
    var $map = $(this._map.Dom);

    // 覆写原生事件监听器,默认函数参数只有event
    div.onmouseenter = function () {
        content.className = 'label label-default';
        this.getElementsByTagName("span")[0].innerHTML = that._text;
    }
    div.onmouseleave = function () {
        content.className = '';
        this.getElementsByTagName("span")[0].innerHTML = '';
    }
    div.ondragstart = function (event) {
        map.disableDragging();
        var pointStart = false;
        var x = event.clientX;
        var y = event.clientY;
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

        // 对外触发节点拖拽完成事件
        var node = $.jstree.reference('#using_json').get_node(that._option.id)
        node.li_attr.lng = pointEnd.lng;
        node.li_attr.lat = pointEnd.lat;
    }
    
    // 给div 提供自由拖拽的能力
    $(div).draggable({
        disabled:true,
        scroll: true,
        opacity: 0.35,
        start: function (event, ui) {           // jquery 事件监听器
            console.log(arguments);
            //map.disableDragging();
            //var pointStart = false;
            //var x = event.clientX;
            //var y = event.clientY;
            //x = x - $map.offset().left;
            //y = y - $map.offset().top;
            //pointStart = map.pixelToPoint(new BMap.Pixel(x, y));
            //that._point = pointStart;               
            //console.log('dragstart:' + pointStart.lng + '  ' + pointStart.lat);
        },
        stop: function (event, ui) {
            console.log(arguments);
            //var pointEnd = false;
            //var x = ui.offset.left + $(this).width() / 2;
            //var y = ui.offset.top + $(this).height() / 2;

            //x = x - $map.offset().left;
            //y = y - $map.offset().top;
            //pointEnd = map.pixelToPoint(new BMap.Pixel(x, y));
            //console.log('dragstop:' + pointEnd.lng + '  ' + pointEnd.lat);
            //that._point = pointEnd;
            //map.enableDragging();
         }
    });

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

// 返回overlay 所属map -jquery对象
CustomOverlay.prototype.getJMap = function () {
    return $(this._map.Ua);
}

// 返回overlay 所属 div-jquery对象
CustomOverlay.prototype.getJDom = function () {
    return $(this._div);
}

CustomOverlay.prototype.disableDragging = function () {
    $(this._div).draggable("disable");
}

CustomOverlay.prototype.enableDragging = function () {
    $(this._div).draggable("enable");
}

// 不能定义 overlay原型对象的同名remove的方法，会覆盖原对象的remove，形成死循环
//CustomOverlay.prototype.remove = function () {
//    window.map.removeOverlay(this);
//    return false;
//}

CustomOverlay.prototype.toggle = function () {
    if (this._span) {
        if (this._span.className === '') {
            $(this._div).trigger('mouseenter');
        }
        else {
            $(this._div).trigger('mouseleave');
        }
    }
}







