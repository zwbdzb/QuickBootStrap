
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S": this.getMilliseconds()
    }
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}

Date.prototype.addDate = function (days) {
    var d = this;
    d.setDate(d.getDate() + days);
    var month = d.getMonth() + 1;
    var day = d.getDate();
    if (month < 10) {
        month = "0" + month;
    }
    if (day < 10) {
        day = "0" + day;
    }
    var val = d.getFullYear() + "" + month + "" + day;
    return val;
}

function getNoOneDay(d) {
    var m = d.getMonth() + 1;
    if (m < 10) {
        m = "0" + m;
    }
    return d.getFullYear() + "" + m + "" + "01";
}


function getLastDay(date) {
    y = date.getFullYear(), m = date.getMonth();
    var lastDay = new Date(y, m + 1, 0);
    return lastDay.format("yyyyMMdd");
}
