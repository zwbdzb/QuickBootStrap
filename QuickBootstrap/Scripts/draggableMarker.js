
function DraggableMarker(point, data) {
    // 怎样在实例化javascript对象的时候，给构造函数的prototype 对象设置参数
    this.__proto__ = new BMap.Marker(point);
    this._point = point;
    this._data = data;

    this.initialize = function (map) {
        this._map = map;
        var div = this._div = document.createElement("div");
        // 行内样式优先于 页面样式，优先于外挂样式
        div.style.position = 'absolute';
        div.style.width = '24px';
        div.style.height = '24px';
        div.style.whiteSpace = "nowrap";
        div.style.fontSize = '18px';
        div.className = $.jsmap.reference(map).options.overlay.style || 'glyphicon glyphicon-facetime-video';       // 'glyphicon glyphicon-map-marker';
        var content = this._span = document.createElement("span");
        div.appendChild(content);

        var that = this;

        // 覆写原生事件监听器,默认函数参数只有event
        div.onmouseenter = function () {
            content.className = 'label label-default';
            this.getElementsByTagName("span")[0].innerHTML = that._data.text;
        }
        div.onmouseleave = function () {
            content.className = '';
            this.getElementsByTagName("span")[0].innerHTML = '';
        }

        map.getPanes().markerPane.appendChild(div);

        // 自定义标注API暂时无法拖拽
        // 给div 提供自由拖拽的能力,由Jquery—UI提供， 这是一个专利点
        $(div).draggable({
            disabled: !$.jsmap.reference(map).status,
            scroll: true,
            opacity: 0.35,
            start: function (event) {
                map.disableDragging();
            },
            stop: function (event, ui) {
                // 利用偏移得到标注物所需的像素
                var $map = $("#" + map.container);
                var x = ui.offset.left + $(this).width() / 2;
                var y = ui.offset.top + $(this).height() / 2;
                x = x - $map.offset().left;
                y = y - $map.offset().top;
                
                // 分别设置拖拽完成之后的地理坐标和像素坐标
                that._point = map.pixelToPoint(new BMap.Pixel(x, y));;

                // 启用地图自身拖拽功能
                map.enableDragging();

                // 事件触发,与地图外元素交互
                var node = $.jstree.reference('#using_json').get_node(that._data.id);
                node.li_attr.lng = that._point.lng;
                node.li_attr.lat = that._point.lat;
            }
        });
        return div;
    }

    this.draw = function () {
        // 地图状态可能发生改变，原标注像素坐标不可用，但此时可反转得到新的像素坐标, 容器会自动计算位置
        var pixel = this._map.pointToOverlayPixel(this._point);
        this._div.style.left = pixel.x + "px";
        this._div.style.top = pixel.y + "px";
    }

    this.enableDragging = function () {
        $(this._div).draggable("enable");
    }

    this.disableDragging = function () {
        $(this._div).draggable("disable");
    }

    this.toggle = function() {
        if (this._span) {
            if (this._span.className === '') {
                $(this._div).trigger('mouseenter');
            } else {
                $(this._div).trigger('mouseleave');
            }
        }
    }
}

