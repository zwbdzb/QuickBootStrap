// 新版本jsTree (Dom 事件， Document事件)

function createTree(container, data, domHandler, docHandler) {
    var conf = {
        'plugins': ['dnd','search','types','contextmenu'],            
        'core': {
            'animation': 0,
            'multipe': 'false',                                    
            'themes': { 'name': 'default',icons:true,'stripes': false, 'responsive': true },
            'data': data,
            'strings': { 'Loading ...': 'Please  wait' }
        },
        'types':{
            'default':{
                'icon':'glyphicon glyphicon-facetime-video',
                'state':'open'
            },
             'group': {
                 'icon': 'glyphicon glyphicon-folder-open'
            }
        },
        'contextmenu': {
            'select_node': false,
            'show_at_node': false,
            'items': {
                "delMarker": {
                    _disabled: false,
                    label: "删除标记",
                    action: function (obj) {
                        var node = $.jstree.reference("#" + container).get_node(obj.reference);
                        node.li_attr.lng = null;
                        node.li_attr.lat = null;
                        /* 以下应该当做事件触发 */
                       var overlay = map.getOverlay(node.id); 						
                        // 这里有 bug
                       map.removeOverlay(overlay);
                        // overlay.remove();
                       markerClusterer.removeMarker(overlay);
                    }

                }
            }
        }
    }
    var tree = $("#" + container).jstree(conf);
    for (var h in domHandler) {
        tree.on(h, domHandler[h]);
    }
    for (var h in docHandler) {
        $(document).bind(h, docHandler[h]);
    }
    return tree;
}