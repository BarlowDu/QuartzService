var hosts = ["localhost:26694"];
var currentHost = "localhost:26694";
var currentScheduler = null;
$(function () {
    initHosts();
    getAllScheduler();
    $("#btnRefresh").click(getAllScheduler);
    $("#btnRevise").click(revise);
    $("#btnReload").click(reloadScheduler);
    $("#btnShow").click(showData);

    //$("#btnUpload").click(uploadZip);
    $("#btnAddOK").click(saveScheduler);

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


    $("#divTb").on("click", "button[cmdStart]", startScheduler);
    $("#divTb").on("click", "button[cmdStop]", stopScheduler);
    $("#divTb").on("click", "button[cmdkill]", killProcess);
    $("#divTb").on("click", "button[cmdUpload]", openCurrentUpload);
    $("#divTb").on("click", "button[cmdEdit]", loadScheduler);
    $("#divTb").on("click", "button[cmdview]", showSchedulerOnHosts);





    $("#divTb").on("click", "button[cmdPauseJob]", pauseJob);
    $("#divTb").on("click", "button[cmdResumeJob]", resumeJob);


    $("#btnStartAll").click(startAllHostScheduler);
    $("#btnStopAll").click(stopAllHostScheduler);
    $("#btnReviseAll").click(reviseAllHostScheduler);
    $("#btnKillAll").click(killAllHostScheduler);
    $("#btnUploadAll").click(uploadAllHostScheduler);

    $("#tbAll").on("click", "button[cmdStart]", startHostScheduler);
    $("#tbAll").on("click", "button[cmdStop]", stopHostScheduler);
    $("#tbAll").on("click", "button[cmdRevise]", reviseHostScheduler);
    $("#tbAll").on("click", "button[cmdKill]", killHostScheduler);
    $("#tbAll").on("click", "button[cmdUpload]", uploadHostScheduler);



});

$.ajaxSetup({
    error: function (data) {
        showWarn("提示", data);
    },
    error: function (xhr, textStatus, errorThrown) {
        console.log(JSON.stringify(textStatus));
        console.log(JSON.stringify(errorThrown));
    },
    beforeSend: function () {
        //$("#modalMask").modal({
        //    show: true, backdrop: 'static'
        //});
    },
    complete: function () {
        //$("#modalMask").modal('hide');
    }
});
function initHosts() {
    $.ajax({
        url: "/Home/GetAllServer",
        type: "get",
        dataType: "json",
        success: function (data) {
            hosts = data.Hosts;
            currentHost = data.CurrentHost;
            var lbl = $("#lblHost");
            lbl.attr("href", "http://" + currentHost).html(currentHost + ' <span class="caret"></span>');

            var ls = $("#lsHosts");
            //ls.empty();
            $.each(hosts, function (i, host) {
                var a = $("<a />").text(host).attr("href", "//" + host);
                ls.append($("<li />").append(a));
            })
        }
    })
}



function getAllScheduler() {
    $("#divTb").load('/Home/AllScheduler')
}
//////////////////////////////////////////////////////////////////////////////

function openCurrentUpload() {

    var self = $(this);
    var btn = $("#btnUpload");
    var sch = getScheduler(self);
    $("#modelTitle").text(sch.schName);
    $("#hidSchId").val(sch.schId);
    $("#form1")[0].reset();
    var save = function () {
        var self = $(this);
        var sch = getScheduler(self);
        $("#form1").ajaxSubmit({
            url: '/Home/UploadApplication',
            success: function (data) {
                showCallbackDefault(data);
                if (data.result) {
                    $("#modalUpload").modal("hide");
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





function startScheduler() {
    var self = $(this);
    var sch = getScheduler(self);

    $.ajax({
        url: '/Home/StartScheduler',
        data: { schId: sch.schId },
        type: 'post',
        dataType: "json",
        success: function (data) {
            showCallbackDefault(data);
        }
    });

}

function stopScheduler() {
    var self = $(this);
    var sch = getScheduler(self);

    $.ajax({
        url: '/Home/ShutDown',
        data: { schId: sch.schId },
        type: 'post',
        dataType: "json",
        success: function (data) {
            showCallbackDefault(data);
        }
    });


}

function killProcess() {
    var self = $(this);
    var sch = getScheduler(self);
    $.ajax({
        url: '/Home/KillProcess',
        data: { schId: sch.schId },
        type: "post",
        dataType: "json",
        success: function (data) {
            showCallbackDefault(data);
        }
    });
}
function revise() {
    $.ajax({
        url: '/Home/Revise',
        type: "post",
        dataType: "json",
        success: function (data) {
            showCallbackDefault(data);
        }
    });
}

function getScheduler(ele) {

    var self = $(ele);
    var p = self.parents("tr");
    return { schId: p.attr("schId"), schName: p.attr("schName") }
}



function reloadScheduler() {
    $.ajax({
        url: '/Home/ReloadScheduler',
        type: "post",
        dataType: "json",
        success: function (data) {
            showCallbackDefault(data);
        }
    });
}
/////////////////////////////////////////////////////
function pauseJob() {
    $.ajax({
        url: '/Home/PauseJob',
        data: getJob($(this)),
        type: "post",
        dataType: "json",
        success: function (data) {
            showCallbackDefault(data);
        }
    });

}

function resumeJob() {
    $.ajax({
        url: '/Home/ResumeJob',
        data: getJob($(this)),
        type: "post",
        dataType: "json",
        success: function (data) {
            showCallbackDefault(data);
        }
    });

}
function getJob(ele) {
    var self = $(ele);
    return { schId: self.attr("schId"), groupName: self.attr("groupname"), jobName: self.attr("jobname") };
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
                showInfo("提示", "保存成功");
                reloadScheduler();
                $("#form2")[0].reset();
            } else {

                showWarn("提示", "保存失败");
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

/////////////////////////////////////////////////////////////////////////
function showSchedulerOnHosts() {

    var self = $(this);
    var sch = getScheduler(self);
    var result = {};
    var callback = function (host, data) {
        result[host] = data;
        var count = 0;
        for (var k in result) {
            count++;
        }
        if (count == hosts.length) {
            openDialogSchedulerOnHosts(sch, result);
        }
    }

    $.each(hosts, function (i, host) {
        $.ajax({
            url: "//" + host + "/api/Scheduler/GetScheduler",
            type: "get",
            data: { schId: sch.schId },
            dataType: "Json",
            success: function (data) {
                callback(host, data);
            }
        })
    });

}

function openDialogSchedulerOnHosts(_scheduler, data) {

    currentScheduler = _scheduler;
    var tb = $("#tbAll");
    tb.empty();
    for (var key in data) {
        var item = data[key];
        var tr = $("<tr />").attr("host", key);
        tr.append($("<td />").text(key));
        tr.append($("<td />").html('<span class="label label-default">' + getStatus(item.Status) + '</span>'));
        tr.append($("<td  class='alert alert-info' />"));
        tr.append(getButtons(item.Status))
        tb.append(tr);
    }
    $("#modalAll").modal({
        show: true,
        keyboard: false

    })
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



/////////////////////////////////////////////////////////////////////////
function showSchedulerOnHosts() {

    var self = $(this);
    var sch = getScheduler(self);
    var result = {};
    var callback = function (host, data) {
        result[host] = data;
        var count = 0;
        for (var k in result) {
            count++;
        }
        if (count == hosts.length) {
            openDialogSchedulerOnHosts(sch, result);
        }
    }

    $.each(hosts, function (i, host) {
        $.ajax({
            url: "//" + host + "/api/Scheduler/GetScheduler",
            type: "get",
            data: { schId: sch.schId },
            dataType: "Json",
            success: function (data) {
                callback(host, data);
            }
        })
    });

}

function openDialogSchedulerOnHosts(_scheduler, data) {

    currentScheduler = _scheduler;
    var tb = $("#tbAll");
    tb.empty();
    for (var key in data) {
        var tr = getHostTr(key, data[key]);
        tb.append(tr);
    }
    $("#modalAll").modal({
        show: true,
        keyboard: false

    })
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
    tr.append($("<td />").text(key));
    tr.append($("<td />").html('<span class="label label-default">' + getStatus(data.Status) + '</span>'));
    tr.append($("<td  class='alert' />"));
    tr.append(getButtonsTd(data.Status));
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

function getUrl(path, host) {
    if (host == null || host == "") {
        return url;
    }
    else {
        return "//" + host + path;
    }
}

///////////////////////

function startHostScheduler() {
    var self = $(this);
    var tr = self.parents("tr");
    var host = tr.attr("host");

    $.ajax({
        url: getUrl('/api/Scheduler/StartScheduler', host),
        data: { schId: currentScheduler.schId },
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
        url: getUrl('/api/Scheduler/ShutDown', host),
        data: { schId: currentScheduler.schId },
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
        url: getUrl('/api/Scheduler/KillProcess', host),
        data: { schId: currentScheduler.schId },
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
        url: getUrl('/api/Scheduler/Revise', host),
        data: { schId: currentScheduler.schId },
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
            url: getUrl('/api/Scheduler/UploadScheduler', host),
            success: function (data) {
                if (data.result) {
                    $("#modalUpload").modal("hide");
                }
                else {
                    showWarn("更新失败");
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

    $("#tbAll button[cmdRevise]").each(function () {
        var self = $(this);
        startHostScheduler.call(self);
    });
}


function stopAllHostScheduler() {

    $("#tbAll button[cmdRevise]").each(function () {
        var self = $(this);
        stopHostScheduler.call(self);
    });
}

function killAllHostScheduler() {

    $("#tbAll button[cmdRevise]").each(function () {
        var self = $(this);
        killHostScheduler.call(self);
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
        $("#tbAll button[cmdRevise]").each(function () {

            var self = $(this);
            var tr = self.parents("tr");
            var host = tr.attr("host")
            $("#form1").ajaxSubmit({
                url: getUrl('/api/Scheduler/UploadScheduler', host),
                success: function (data) {
                    if (data.result == false) {
                        tr.find("td:eq(2)").addClass("alert-warning").text(data.msg);
                    }
                }
            });
        });

    };
    btn.unbind().click(save);
    $("#modalUpload").modal({
        show: true,
        keyboard: false

    });
}
