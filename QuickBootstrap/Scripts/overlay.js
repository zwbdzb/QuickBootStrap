
// 复杂的自定义覆盖物
function ComplexCustomOverlay(point, option) {
	this._point = point;
	this._option = option;
}

if (window.BMap && window.BMap.Overlay) {
	ComplexCustomOverlay.prototype = new BMap.Overlay();
}

ComplexCustomOverlay.prototype.initialize = function (map) {
	this._map = map;
	var div = this._div = document.createElement("div");

	div.style.position = "absolute";
	div.style.zIndex = BMap.Overlay.getZIndex(this._point.lat);
	div.style.backgroundColor = "#EE5D5B";
	div.style.border = "1px solid #BC3B3A";
	div.style.color = "white";
	div.style.height = "18px";
	div.style.padding = "2px";
	div.style.lineHeight = "18px";
	div.style.whiteSpace = "nowrap";
	div.style.fontSize = "12px";
	div.className = "marker";

	var content = this._span = document.createElement("span");

	content.innerHTML = this._option.DN;  // that._option.DeviceName;  
	div.appendChild(content);

	var that = this;

	// #region 页面箭头部分
	var arrow = this._arrow = document.createElement("div");
	arrow.style.background = "url(http://map.baidu.com/fwmap/upload/r/map/fwmap/static/house/images/label.png) no-repeat";
	arrow.style.position = "absolute";
	arrow.style.width = "11px";
	arrow.style.height = "10px";
	arrow.style.top = "22px";
	arrow.style.left = "10px";
	arrow.style.overflow = "hidden";
	div.appendChild(arrow);
	//#endregion      

	div.onmouseenter = function () {
		if (flag == 0) {
			this.style.backgroundColor = "#6BADCA";
			this.style.borderColor = "#0000ff";
			this.getElementsByTagName("span")[0].innerHTML = Parse(that._option);
			arrow.style.backgroundPosition = "0px -20px";
		}
	}

	div.onmouseleave = function () {
		if (flag == 0) {
			this.style.backgroundColor = "#EE5D5B";
			this.style.borderColor = "#BC3B3A";
			this.getElementsByTagName("span")[0].innerHTML = that._option.DN //that._option.SN;            //+ box.innerHTML;
			arrow.style.backgroundPosition = "0px 0px";
		}
	}

	div.ondragstart = function (event, ui) {
		map.disableDragging();

		//  从拖拽释放像素位置求得地理位置信息	
		var pt;
		var x = event.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
		var y = event.clientY + document.body.scrollTop + document.documentElement.scrollTop;
		var position = $('#allmap').position();
		x = x - position.left;
		y = y - position.top;
		pt = map.pixelToPoint(new BMap.Pixel(x, y));
		console.log('dragstart:' + pt.lng + '  ' + pt.lat);
	}

	div.ondragstop = function (event, ui) {
		//  从拖拽释放div箭头位置求得地理位置信息	
		var pt = false;
		var x = ui.offset.left + 10;
		var y = ui.offset.top + 22 + 10;
		var position = $('#allmap').position();
		x = x - position.left;
		y = y - position.top;
		pt = map.pixelToPoint(new BMap.Pixel(x, y));
		console.log('dragstop:' + pt.lng + '  ' + pt.lat);
		that._point = pt;

		var dataJson = that._option;
		updataUsedNode(dataJson.id.substring(0, 36), pt);     // 更新节点位置
		$.ajax({
			url: "/Device/DeviceMarker?id=" + dataJson.id.substring(0, 36) + "&&lat=" + that._point.lat + "&&lng=" + that._point.lng,
			type: "POST",
			dataType: "json",
			success: function (data) {
				if (!data.Result) {
					return;
				}
				var node = $('#GroupDeviceList').jstree("get_node", dataJson.id.substring(0, 36));
				node.li_attr.lng = that._point.lng;
				node.li_attr.lat = that._point.lat;
			}
		});

		map.enableDragging();
	}

	// 根据状态确定DIV是否可以拖拽
	$(div).draggable({ scroll: true, opacity: 0.35, disabled: (flag ? false : true) });

	map.getPanes().markerPane.appendChild(div);
	return div;
}

ComplexCustomOverlay.prototype.getPosition = function () {
	return this._point;
}

// 这个一定要
ComplexCustomOverlay.prototype.getMap = function () {
	return this._map;
}

ComplexCustomOverlay.prototype.draw = function () {
	var map = this._map;
	// console.log("marker-lbs:" + this._point.lng + ',' + this._point.lat);
	var pixel = map.pointToOverlayPixel(this._point);
	this._div.style.left = pixel.x - parseInt(this._arrow.style.left) + "px";
	this._div.style.top = pixel.y - 30 + "px";
}

// 自定义拖拽能力  (这个项目是没有使用的)
ComplexCustomOverlay.prototype.disableDragging = function () {
	$(this._div).draggable("destroy");			//  调用"disable" 方法 要变成灰色
}

ComplexCustomOverlay.prototype.enableDragging = function () {
	$(this._div).draggable({ disabled: false });
}

ComplexCustomOverlay.prototype.Remove = function () {
	map.removeOverlay(this);
}

ComplexCustomOverlay.prototype.toggle = function () {
	if (this._div) {
		if (this._div.style.display == "") {
			this.hide();
		}
		else {
			this.show();
		}
	}
}

$('.marker').draggable();



function Parse(dataJson) {
	var sContent = '';
	if (dataJson.channel !== "") {
		sContent = dataJson.DN + '	';
		var channels = dataJson.channel.split(',');
		for (var i = 0; i < channels.length; i++) {
			sContent += "<a style='color:white;' onclick=\"PlayVideo('" + dataJson.id + "'," + channels[i].split(':')[0] + "\);\">" + channels[i] + "</a>" + " ";
		}
	} else {
		sContent = "<a style='color:white;text-decorationnone;' onclick=\"PlayVideo('" + dataJson.id + "',0 )\"   >" + dataJson.DN + "</a>";
	}
	return sContent;
}

function PlayVideo(id, ch) {
	// 触发 JStree的交互
	$.jstree.reference("#GroupDeviceList").deselect_all();
	$.jstree.reference("#GroupDeviceList").select_node(id);

	var d = FindDevice(id); 					//	得到设备
	if (typeof (isFromPCClient) !== "undefined" && isFromPCClient == true) {
		try {
			//  使用脚本代码中的window.external 对象访问指定对象的公开属性和方法
			window.external.JSCallCppCB(0, ch, d.SN);
		} 			// 与PC端约定的调用函数 JSCallCppCB
		catch (e) {

			$('#devConfirm p').html("没有找到PC客户端调用的API");
			$('#devConfirm').dialog({
				autoOpen: true,
				resizable: false,
				height: 140,
				modal: true,
				buttons: {
					"确定": function () {
						$(this).dialog('close');
					}
				}
			});
		}
	} else {
		if (!d.online) {
			$("#devConfirm").dialog({
				autoOpen: true,
				resizable: false,
				height: 140,
				modal: true,
				buttons: {
					"确定": function () {
						$(this).dialog("close");
						PlayDevice(id, ch);
					},
					"取消": function () {
						$(this).dialog("close");
						return false;
					}
				}
			});
		}
		else {
			PlayDevice(id, ch);
		}
		// 播放之后写入本地缓存( 设备ID)
		SetRecentlyUserDevice(id);

	}
};





