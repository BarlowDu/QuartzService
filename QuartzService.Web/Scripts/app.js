var currentScheduler = null;
var schedulers = {};
/*
{"SchedulerName":{"schedulerId":1,"list":{"localhost":{},"localhost1":{}},"summary":{"error":0,"none":0,"running":0,"stop":0,"processRunning":0}}}
*/
var schedulersData = [];
$(function () {
    getAllScheduler();
    $("#btnRefresh").click(getAllScheduler);
    $("#btnShow").click(showData);
    $("#btnAdd").click(function () {
        $("#hidSchedulerId").val(0);
        $("#txtSchedulerName").val("");
        $("#txtDirectory").val("");
        $("#txtFileName").val("");
        $("#txtPort").val("");
        $("#schTitle").text("添加任务");
        $("#modalScheduler").modal({
            show: true,
            keyboard: false

        });
    });

    $("#btnSaveOK").click(saveScheduler);
    /////////////////

    $("#tb").on("click", "button[cmdStart]", startScheduler);
    $("#tb").on("click", "button[cmdStop]", stopScheduler);
    $("#tb").on("click", "button[cmdView]", showSchedulerOnHosts);
    $("#tb").on("click", "button[cmdEdit]", loadScheduler);
    $("#tb").on("click", "button[cmdRevise]", revise);





    $("#btnStartAll").click(startAllHostScheduler);
    $("#btnStopAll").click(stopAllHostScheduler);
    $("#btnReviseAll").click(reviseAllHostScheduler);

    $("#btnUploadAll").click(uploadAllHostScheduler);
    $("#btnSchedulerClose").click(closeSchedulerOnHosts);

    $("#tbAll").on("click", "button[cmdStart]", startHostScheduler);
    $("#tbAll").on("click", "button[cmdStop]", stopHostScheduler);
    $("#tbAll").on("click", "button[cmdRevise]", reviseHostScheduler);
    $("#tbAll").on("click", "button[cmdKill]", killHostScheduler);
    $("#tbAll").on("click", "button[cmdUpload]", uploadHostScheduler);



});

$.ajaxSetup({
    async: false,
    error: function (data) {
        showWarn("提示", data);
    },
    error: function (xhr, textStatus, errorThrown) {
        console.log(JSON.stringify(textStatus));
        console.log(JSON.stringify(errorThrown));
    }

});



///////////////////////////////////////////////////////////

function getAllScheduler() {
    $("#tb").empty();
    $.ajax({
        url: "/api/AllServer/ReviseAll",
        type: 'get',
        dataType: "json",
        success: function (data) {
            schedulers = data;
            showAllScheduler();
        }
    })
}

function getAllHostScheduler(path) {
    var hostCount = 0;
    $.ajax({
        url: path,
        type: "get",
        dataType: "json",
        error: function () {
        },
        success: function (data) {
            schedulers = data;
            showAllScheduler();
        }
    })

}

function showAllScheduler() {
    var tb = $("#tb");
    for (var key in schedulers) {
        var scheduler = schedulers[key];
        var tr = getSchedulerTr(key, scheduler);
        tb.append(tr);
    }
}
function getSchedulerTr(schedulerName, scheduler) {
    var tr = $("<tr />");
    var td1 = $("<td />").text(schedulerName);
    var td2 = $("<td />");
    var td3 = $("<td />");
    var summary = scheduler.summary;

    td2.append($('<div><span class="label label-default">Error</span><span class="label label-info">' + summary.error + '</span></div>'))
       .append($('<div><span class="label label-default">None</span><span class="label label-info">' + summary.none + '</span></div>'))
       .append($('<div><span class="label label-default">Stop</span><span class="label label-info">' + summary.stop + '</span></div>'))
       .append($('<div><span class="label label-default">Running</span><span class="label label-info">' + summary.running + '</span></div>'))
       .append($('<div><span class="label label-default">ProcessRunning</span><span class="label label-info">' + summary.processRunning + '</span></div>'));

    td3.append($('<button class="btn btn-default" cmdStart>运行</button>'))
       .append($('<button class="btn btn-default" cmdStop>停止</button>'))
       .append($('<button class="btn btn-default" cmdView>查看所有</button>'))
       .append($('<button class="btn btn-default" cmdRevise>校验</button>'))
       .append($('<button class="btn btn-default" cmdEdit>编辑</button>'));
    tr.append(td1).append(td2).append(td3).attr("schId", scheduler.schedulerId).attr("schName", schedulerName);
    return tr;
}
/////////////////////////////////////////


function showData() {
    $.ajax({
        url: "/Home/GetAllSchedulerData",
        type: 'get',
        dataType: "json",
        success: function (data) {
            var ls = $("#lsData");
            ls.empty();
            $.each(data, function (index, item) {
                var tr = $("<tr />");
                var td1 = $("<td />").text(item.SchedulerId);
                var td2 = $("<td />").text(item.SchedulerName);
                var td3 = $("<td />").text(item.Directory);
                var td4 = $("<td />").text(item.FileName);
                var td5 = $("<td />").text(item.Port);
                tr.append(td1).append(td2).append(td3).append(td4).append(td5);
                ls.append(tr);
            });
            $("#modalData").modal({
                show: true,
                keyboard: false

            });
        }
    })
}

function saveScheduler() {
    if (validate() == false) {
        return;
    }
    $("#form2").ajaxSubmit({
        url: "/Home/SaveScheduler",
        type: "post",
        success: function (data) {
            if (data.result) {
                $("#modalScheduler").modal("hide");
                showInfo("提示", data.msg);
                getAllScheduler();
                $("#form2")[0].reset();
            } else {

                showWarn("提示", "保存失败:" + data.msg);
            }
        }
    });
}

function validate() {
    var result = true;
    var schName = $("#txtSchedulerName").val().trim();
    var dir = $("#txtDirectory").val().trim();
    var file = $("#txtFileName").val().trim();
    var port = $("#txtPort").val().trim();
    var setError = function (id, bln) {
        if (bln) {
            $("#" + id + "").parent().removeClass("has-error");
        } else
            $("#" + id + "").parent().addClass("has-error");
    }


    if (schName == "") {
        result = false;
        $("#txtSchedulerName").parent().addClass("has-error");
    } else {
        $("#txtSchedulerName").parent().removeClass("has-error");
    }



    if (dir == "") {
        result = false;
        $("#txtDirectory").parent().addClass("has-error");
    } else {
        $("#txtDirectory").parent().removeClass("has-error");
    }



    if (file == "") {
        result = false;
        $("#txtFileName").parent().addClass("has-error");
    } else {
        $("#txtFileName").parent().removeClass("has-error");
    }



    if (isNaN(parseInt(port))) {
        result = false;
        $("#txtPort").parent().addClass("has-error");
    } else {
        $("#txtPort").parent().removeClass("has-error");
    }

    return result;
}


/////////////////////////////////////////
function showSchedulerOnHosts() {

    var self = $(this);
    var sch = getScheduler(self);

    currentScheduler = sch;
    $("#lblCurrentSchName").text(currentScheduler.schName);
    var tb = $("#tbAll");
    tb.empty();
    var _sch = schedulers[sch.schName];
    for (var key in _sch.list) {
        var data = _sch.list[key];
        var tr = getHostTr(key, data);
        tb.append(tr);
    }
    $("#modalAll").modal({
        backdrop: 'static',
        show: true,
        keyboard: false

    })
    return;


}


function getStatus(intStatus) {
    switch (intStatus) {
        case 1:
            return "Stop";
        case 2:
            return "Running";
        case 3:
            return "ProcessRunning";
        default:
            return "None"
    }
}
function getHostTr(key, data) {

    var tr = $("<tr />").attr("host", key);
    if (data != null) {
        tr.append($("<td />").text(key));
        tr.append($("<td />").html('<span class="label label-default">' + getStatus(data.Status) + '</span>'));
        tr.append($("<td  class='alert' />"));
        tr.append(getButtonsTd(data.Status));
    }
    else {
        tr.append($("<td colspan='4' />").text(key + "加载失败!"));
    }
    return tr

}
function getButtonsTd(intStatus) {
    var td = $("<td />");
    if (intStatus == 1) {
        td.append($('<button class="btn btn-default" cmdStart >运行</button>'));
        td.append($('<button class="btn btn-default" cmdUpload >更新</button>'));
    }
    else if (intStatus == 2) {
        td.append($('<button class="btn btn-default" cmdStop >停止</button>'));

    }
    else if (intStatus == 3) {
        td.append($('<button class="btn btn-default" cmdKill >强制关闭</button>'));

    }

    td.append($('<button class="btn btn-default" cmdRevise >校验</button>'));

    return td;
}
function getButtons(intStatus) {
    var td = $("<td />");
    if (intStatus == 1) {
        td.append($('<button class="btn btn-default" cmbStart >运行</button>'));
        td.append($('<button class="btn btn-default" cmdupload >更新</button>'));
    }
    else if (intStatus == 2) {
        td.append($('<button class="btn btn-default" cmdstop >停止</button>'));

    }
    else if (intStatus == 3) {
        td.append($('<button class="btn btn-default" cmdkill >强制关闭</button>'));

    }

    td.append($('<button class="btn btn-default" cmdrevise >校验</button>'));

    return td;
}


function closeSchedulerOnHosts() {
    var btnRevise = $("#tb tr[schId=" + currentScheduler.schId + "] button[cmdRevise]");
    if (btnRevise.length > 0) {
        revise.call(btnRevise);
    }
}
/////////

function loadScheduler() {
    var self = $(this);
    var schId = self.parents("tr").attr("schid");
    $("#schTitle").text("编辑任务");
    $.ajax({
        url: "/Home/GetScheduler",
        data: { schId: schId },
        type: "get",
        dataType: "json",
        success: function (data) {
            if (data == null) { return; }
            $("#hidSchedulerId").val(data.SchedulerId);
            $("#txtSchedulerName").val(data.SchedulerName);
            $("#txtDirectory").val(data.Directory);
            $("#txtFileName").val(data.FileName);
            $("#txtPort").val(data.Port);
            $("#modalScheduler").modal({
                show: true,
                keyboard: false

            });
        }
    });
}
function revise() {
    var self = $(this);
    var tr = self.parents("tr");
    var sch = getScheduler(self);

    $.ajax({
        url: "/api/AllServer/Revise",
        data: { schId: sch.schId },
        type: "post",
        dataType: "json",
        error: function () {
        },
        success: function (data) {
            schedulers[sch.schName] = data;
            var nTr = getSchedulerTr(sch.schName, data);
            tr.replaceWith(nTr);
        }
    });
}
function startScheduler() {
    var self = $(this);
    var tr = self.parents("tr");
    var sch = getScheduler(self);

    $.ajax({
        url: "/api/AllServer/StartAllHostScheduler",
        data: { schId: sch.schId, },
        type: "post",
        dataType: "json",
        error: function () {
        },
        success: function (data) {
            revise.call(self);
        }
    });
}

function stopScheduler() {
    var self = $(this);
    var tr = self.parents("tr");
    var sch = getScheduler(self);

    $.ajax({
        url: "/api/AllServer/StopAllHostScheduler",
        data: { schId: sch.schId, },
        type: "post",
        dataType: "json",
        error: function () {
        },
        success: function (data) {
            revise.call(self);
        }
    });
}
function getScheduler(ele) {

    var self = $(ele);
    var p = self.parents("tr");
    return { schId: p.attr("schId"), schName: p.attr("schName") }
}
////////////////////////////////////////
///////////////////////

function startHostScheduler() {
    var self = $(this);
    var tr = self.parents("tr");
    var host = tr.attr("host");

    $.ajax({
        url: '/api/AllServer/StartHostScheduler',
        data: { schId: currentScheduler.schId, host: host },
        type: 'post',
        dataType: "json",
        success: function (data) {
            if (data.result) {
                reviseHostScheduler.call(self);
            }
            else {
                tr.find("td:eq(2)").addClass("alert-warning").text(data.msg);
            }
        }
    });

}
function stopHostScheduler() {
    var self = $(this);
    var tr = self.parents("tr");
    var host = tr.attr("host");

    $.ajax({
        url: '/api/AllServer/StopHostScheduler',
        data: { schId: currentScheduler.schId, host: host },
        type: 'post',
        dataType: "json",
        success: function (data) {
            if (data.result) {
                reviseHostScheduler.call(self);
            }
            else {
                tr.find("td:eq(2)").addClass("alert-warning").text(data.msg);
            }
        }
    });

}

function killHostScheduler() {
    var self = $(this);
    var tr = self.parents("tr");
    var host = tr.attr("host");
    $.ajax({
        url: '/api/AllServer/KillProcess',
        data: { schId: currentScheduler.schId, host: host },
        type: 'post',
        dataType: "json",
        success: function (data) {
            if (data.result) {
                reviseHostScheduler.call(self);
            }
            else {
                tr.find("td:eq(2)").addClass("alert-warning").text(data.msg);
            }
        }
    });

}

function reviseHostScheduler() {
    var tr = $(this).parents("tr");
    var host = tr.attr("host");
    $.ajax({
        url: '/api/AllServer/ReviseHostScheduler',
        data: { schId: currentScheduler.schId, host: host },
        type: 'post',
        dataType: "json",
        success: function (data) {
            if (data != null) {
                var nTr = getHostTr(host, data);
                tr.replaceWith(nTr);
            }
        }
    });

}
function uploadHostScheduler() {



    var self = $(this);
    var tr = self.parents("tr");
    var host = tr.attr("host")

    var btn = $("#btnUpload");

    $("#modelTitle").text(currentScheduler.schName);
    $("#hidSchId").val(currentScheduler.schId);
    $("#form1")[0].reset();
    var save = function () {

        $("#form1").ajaxSubmit({
            url: '/api/AllServer/UploadHostScheduler?host=' + host,
            //data:{host:host},
            error: function (xhr, textStatus, errorThrown) {
                var msg = JSON.stringify(textStatus) + JSON.stringify(errorThrown);
                showWarn("提示", msg);
            },
            success: function (data) {
                if (data.result) {
                    $("#modalUpload").modal("hide");
                }
                else {
                    showWarn("提示", "更新失败");
                }
            }
        });

    };
    btn.unbind().click(save);
    $("#modalUpload").modal({
        show: true,
        keyboard: false

    });

}

////////////////////

function startAllHostScheduler() {

    $.ajax({
        url: "/api/AllServer/StartAllHostScheduler",
        data: { schId: currentScheduler.schId, },
        type: "post",
        dataType: "json",
        error: function () {
        },
        success: function (data) {
            reviseAllHostScheduler();
        }
    });
}


function stopAllHostScheduler() {
    $.ajax({
        url: "/api/AllServer/StopAllHostScheduler",
        data: { schId: currentScheduler.schId, },
        type: "post",
        dataType: "json",
        error: function () {
        },
        success: function (data) {
            reviseAllHostScheduler();
        }
    });
}

function reviseAllHostScheduler() {

    $("#tbAll button[cmdRevise]").each(function () {
        var self = $(this);
        reviseHostScheduler.call(self);
    });
}

function uploadAllHostScheduler() {


    var btn = $("#btnUpload");

    $("#modelTitle").text(currentScheduler.schName);
    $("#hidSchId").val(currentScheduler.schId);

    $("#form1")[0].reset();
    var save = function () {

        $("#modalUpload").modal("hide");

        var self = $(this);
        var tr = self.parents("tr");
        var host = tr.attr("host")
        $("#form1").ajaxSubmit({
            url: '/api/AllServer/UploadAllScheduler',
            error: function (xhr, textStatus, errorThrown) {
            },
            success: function (data) {
                $.each(data, function (i, item) {
                    if (item.result == false) {

                        $("#tbAll>tr[host='" + item.host + "']>td:eq(2)").addClass("alert-warning").text(data.msg);
                    }

                })
            }
        });

    };
    btn.unbind().click(save);
    $("#modalUpload").modal({
        show: true,
        keyboard: false

    });
}



//////////////////////////////////////////////////////////////////////////

function showCallbackDefault(callback) {
    if (callback.result) {
        getAllScheduler();
    } else {
        showWarn("提示", callback.msg);
    }
}
function showWarn(title, msg) {
    $("#spanWarnTitle").text(title);
    $("#divWarnMsg").text(msg);
    $("#modalWarn").modal({
        show: true,
        keyboard: false
    })
}



function showInfo(title, msg) {
    $("#spanInfoTitle").text(title);
    $("#divInfoMsg").text(msg);
    $("#modalInfo").modal({
        show: true,
        keyboard: false
    })
}

///////////////////////////////////////////////////////////////////////