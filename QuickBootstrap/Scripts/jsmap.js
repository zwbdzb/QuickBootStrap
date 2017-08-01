
$.jsmap = {
    version : '0.0.1',
    defaults: {
        'controls': { 
                MapTypeControl: { isopen: 1 },
                OverviewMapControl: { isopen: 1 },
                ScaleControl: { isopen: 1 },
                NavigationControl: { isopen: 1 }
        },
        'overlay': {
            'icon': 'glyphicon glyphicon-facetime-video',
            'style':undefined
        }
    },
    controls: {
        
    }
};

/**
 * creates a jsmap instance
 * @name $.jsmap.create(el [, options])
 * @param {DOMElement|jQuery|String} el the element to create the instance on, can be jQuery extended or a selector
 * @param {Object} options options for this instance (extends `$.jsmap.defaults`)
 * @return {jsmap} the new instance
 */
$.jsmap.create = function(el, options) {

    var options = $.extend(true, {}, $.jsmap.defaults, options);

    var jsmap = new $.jsmap.core(el, options);
   
    return jsmap;
}

/**
	 * the jsmap class constructor, used only internally
	 * @private
	 * @name $.jsmap.core(id)
	 * @param {Number} id this instance's index
	 */
$.jsmap.core = function(el,options) {
    var map = new BMap.Map(el);

    map.container = el;
    this.map = map;
    this.userMarkers = [];
    this.options = options;

    var local = new BMap.LocalCity();
    local.get(function (localCityResult) {
        map.centerAndZoom(localCityResult.center, 13);
    });

    map.enableScrollWheelZoom(true);   
    map.enableDragging();				
    map.enableScrollWheelZoom();		
    map.enableDoubleClickZoom();		
    map.enableKeyboard();

    for (ctl in  options.controls) {
        //  TODO 应该是循环controls，根据字符串实例化出插件
    }
    
    map.addControl(new BMap.MapTypeControl({ anchor: BMAP_ANCHOR_TOP_LEFT } ));
    var ctrl_nav = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, type: BMAP_NAVIGATION_CONTROL_LARGE });
    map.addControl(ctrl_nav);
    var ctrl_ove = new BMap.OverviewMapControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, isOpen: 0 });
    map.addControl(ctrl_ove);
    var ctrl_sca = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT });
    map.addControl(ctrl_sca);


    var statusCtrl = new StatusControl();
    map.addControl(statusCtrl);
    var treeControl = new TreeControl(options.controls.tree);
    map.addControl(treeControl);
};

$.jsmap.core.prototype.findOverlay = function (id) {
    var overlays = this.map.getOverlays();
    for (x in overlays) {
        if (!!overlays[x]._data && overlays[x]._data.id === id)
            return overlays[x];
    }
};

$.jsmap.core.prototype.getUserMarkers = function () {
    return this.userMarkers;
}

$.jsmap.core.prototype.loadMarker = function (point, obj,style) {
    var myOverlay = new DraggableMarker(point, obj, style);
    this.map.addOverlay(myOverlay);
    this.userMarkers.push(myOverlay);
    return true;
}

/* map container ， internal map*/
$.jsmap.reference = function (args) {
    // TODO 怎么根据id，dom返回 jsmap对象
    return window.jsmap;
}

function StatusControl() {
    this.defaultAnchor = BMAP_ANCHOR_TOP_RIGHT;
    this.defaultOffset = new BMap.Size(540,10);
}
StatusControl.prototype = new BMap.Control();
StatusControl.prototype.initialize = function (map) {
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
    div.onclick = function () {
        var markers = jsmap.userMarkers;
        if ( !$.jsmap.reference(map).status ) {
            $.jsmap.reference(map).status = true;

            div.childNodes[0].textContent = "编辑模式";
            div.className = 'glyphicon glyphicon-pencil';
            //  改变所有marker的 编辑状态,应该把设备列表做成map里面，形成一个大的map控件
            $.each(markers, function (i,n) {
                if (n.hasOwnProperty('_data')) {
                    n.enableDragging();
                }
            });
        } else {
            $.jsmap.reference(map).status = false;
            div.childNodes[0].textContent = "正常模式";
            div.className = 'glyphicon glyphicon-star';
            // 改变所有marker的编辑状态
            $.each(markers, function (i, n) {
                if (n.hasOwnProperty('_data')) {
                    n.disableDragging();
                }
            });
        }
    }
    map.getContainer().appendChild(div);
    return div;
}

/**
 * ### tree control
 *
 * Enables jstree in baidu map,load default plugins
 */

/**
 * stores all defaults for the tree control
 * @name $.jsmap.defaults.tree
 * @plugin dnd
 */
$.jsmap.defaults.tree = {
    title: '地图树控件标题',
    location: {
        defaultAnchor: BMAP_ANCHOR_TOP_RIGHT,
        defaultOffset: new BMap.Size(20, 20)
    },
    treebody: {
        plugins: ['dnd', 'search', 'types', 'contextmenu'],
        core: {
            'animation': 0,
            'multipe': 'false',
            'themes': { 'name': 'default', icons: true, 'stripes': false, 'responsive': true },
            'strings': { 'Loading ...': 'Please  wait' }
        },
        types: {
            'default': {
                'icon': 'glyphicon glyphicon-facetime-video',
                'state': 'open'
            },
            group: {
                icon: 'glyphicon glyphicon-folder-open'
            }
        },
        contextmenu: {
            'select_node': false,
            // 'show_at_node': tree,
            'items': {
                "delMarker": {
                    _disabled: false,
                    label: "删除标记",
                    icon: 'glyphicon glyphicon-remove',
                    action: function (obj) {
                        var container = obj.reference.context.id;
                        var node = $.jstree.reference("#" + container).get_node(obj.reference);
                        node.li_attr.lng = null;
                        node.li_attr.lat = null;
                        /* TODO 以下应该当做事件触发 */
                        var overlay = $.jsmap.reference(map).map.getOverlay(node.id);
                        map.removeOverlay(overlay);
                    }

                }
            }
        },
    }
}

$.jsmap.controls.tree = function (options, parent) {
    var options = $.fn.extend(true, {}, $.jsmap.defaults.tree, options);
    var jsmap = $.jsmap.reference(parent);

    var el = options.treeId;
    var domHandlers = {
        'loaded.jstree': function (e, tree) {
            tree.instance.open_all();
            $.each(tree.instance._model.data, function (i, n) {
                if (n.li_attr && n.li_attr.lng && n.li_attr.lat) {
                    // TODO 这里也应该用事件通知的方式来完成
                    var point = new BMap.Point(n.li_attr.lng, n.li_attr.lat);
                    $.jsmap.reference(parent).loadMarker(point, n);
                }
            });

        },
        'select_node.jstree': function (e, data) {
            var node = data.node;
            $('#' + el).jstree("toggle_node", node);
            if (node.parent === '#') {
                $.jsmap.reference(parent).map.reset();
                return false;
            }
            if (node.li_attr.lng && node.li_attr.lat) {
                var pt = new BMap.Point(node.li_attr.lng, node.li_attr.lat);
                var jsmap = $.jsmap.reference(parent);
                jsmap.map.panTo(pt);
                var overlay = jsmap.findOverlay(node.li_attr.id);
                overlay.toggle();
             //   setTimeout(overlay.toggle, 2000);  // 是错误的，setTimeOut只是指定一个函数在2000ms之后执行，并没有指定scope,执行时函数的执行环境是window，以下写法形成一个闭包
                setTimeout(overlay.toggle.bind(overlay), 2000);   // 这是以下写法另外一种优雅的写法
                //setTimeout(function () {
                //    overlay.toggle();
                //}, 3000);
            }
        },
        'open_node.jstree': function (e, data) {
            var node = data.node;
            data.instance.set_icon(node, 'glyphicon glyphicon-folder-open');
        },
        'close_node.jstree': function (e, data) {
            var node = data.node;
            data.instance.set_icon(node, 'glyphicon glyphicon-folder-close');
        }
    };
    var docHandlers = {
        'dnd_stop.vakata': function (e, data) {

            var node = $.jstree.reference('#' + data.data.obj.context.id).get_node(data.data.nodes[0]);
            if (node.type === 'group')
                return false;
            var jsmap = $.jsmap.reference(parent);
            if (!jsmap.status || node.li_attr.lat && node.li_attr.lng) {
                return false;
            }
            /* 以下将采用事件触发的方式 */
            var pt = null;
            var x = data.event.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
            var y = data.event.clientY + document.body.scrollTop + document.documentElement.scrollTop;

            var jsmap = $.jsmap.reference(parent);
            var $map = $("#" + jsmap.map.container);
            var x1 = x - $map.offset().left;
            var y1 = y - $map.offset().top;

            var x2 = x - ($map.offset().left + $map.width());
            var y2 = y - ($map.offset().top + $map.height());
            if ((x1 < 0 || y1 < 0) || (x2 > 0 || y2 > 0)) {
                return false;
            }

            var point = jsmap.map.pixelToPoint(new BMap.Pixel(x1, y1));
            node.li_attr.lng = point.lng;
            node.li_attr.lat = point.lat;

            if (jsmap.loadMarker(point, node) === true) {
                // TODO something  change the jstree node data
                return true;
            }
        },
        'context_show.vakata': function (e, data) { // 控制是否显示 右键菜单
            var jstree = $.jstree.reference('#' + data.reference.context.id);
            var node = jstree.get_node(data.reference);
            if (jsmap.status === 0 || node.type === 'group' || !node.li_attr.lng || !node.li_attr.lat) {
                $(data.element).hide();
                return false;
            }

        }
    };

    var tree = $("#" + el).jstree(options.treebody);

    for (var e in domHandlers) {
        tree.on(e, domHandlers[e]);
    }
    for (var e in docHandlers) {
        $(document).on(e, docHandlers[e]);
    }
    return tree;
};

// 直接实例化出tree控件
function TreeControl(options) {
    options = $.fn.extend(true, {}, $.jsmap.defaults.tree, options);
    this.defaultAnchor = options.location.defaultAnchor;
    this.defaultOffset = options.location.defaultOffset;
    this.title = options.title;
    this.options = options;
}

TreeControl.prototype = new BMap.Control();
// map添加控件的时候开始绘制
TreeControl.prototype.initialize = function (map) {

    var div = document.createElement("div");
    div.className = 'panel panel-info';

    var title = document.createElement("div");
    var text = document.createElement("h3");
    $(text).addClass("panel-title").text( this.title).attr( {'data-toggle':'tooltip', 'data-placement':'top','title':'单击切换编辑模式'});
    $(title).addClass("panel-heading").append(text);
    div.appendChild(title);

    var body = document.createElement("div");
    $(body).addClass("panel-body").append("<div id='using_json'></div>");
    div.appendChild(body);
    
    map.getContainer().appendChild(div);

    //  必须产生DOM，才能加载jstree数据
    $.jsmap.controls.tree(this.options, map.container);
    return div;
}