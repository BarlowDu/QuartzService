$(function () {
    getAllScheduler();
    $("#btnRefresh").click(getAllScheduler);
    $("#btnRevise").click(revise);
    $("#btnReload").click(reloadScheduler);

    $("#btnUpload").click(uploadZip);


    $("#divTb").on("click", "button[cmdStart]", startScheduler);


    $("#divTb").on("click", "button[cmdStop]", stopScheduler);

    $("#divTb").on("click", "button[cmdkill]", killProcess);

    $("#divTb").on("click", "button[cmdUpload]", openUploadModal);



    $("#divTb").on("click", "button[cmdPauseJob]", pauseJob);


    $("#divTb").on("click", "button[cmdResumeJob]", resumeJob);


});

$.ajaxSetup({
    error: function (data) {
        showWarn("提示", data);
    },
    beforeSend: function () { $("#modalMask").modal({ show: true, backdrop: 'static' }); },
    complete: function () { $("#modalMask").modal('hide'); }
});

function openUploadModal() {

    var self = $(this);
    var sch = getScheduler(self);
    $("#modelTitle").text(sch.schName);
    $("#hidSchId").val(sch.schId);
    $("#modalUpload").modal({
        show: true,
        keyboard: false

    });
}

function uploadZip() {
    var self = $(this);
    var sch = getScheduler(self);
    $("#form1").ajaxSubmit({
        url: 'Home/UploadApplication',
        success: function (data) {
            showCallbackDefault(data);
            $("#modalUpload").modal("hide");
        },
        complete: function () {
            $("#form1")[0].reset();

        }
    });

}

function startScheduler() {
    var self = $(this);
    var sch = getScheduler(self);

    $.ajax({
        url: 'Home/StartScheduler',
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
        url: 'Home/ShutDown',
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
        url: 'Home/KillProcess',
        data: { schId: sch.schId },
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


function revise() {
    $.ajax({
        url: 'Home/Revise',
        type: "post",
        dataType: "json",
        success: function (data) {
            showCallbackDefault(data);
        }
    });
}

function reloadScheduler() {
    $.ajax({
        url: 'Home/ReloadScheduler',
        type: "post",
        dataType: "json",
        success: function (data) {
            showCallbackDefault(data);
        }
    });
}

function pauseJob() {
    $.ajax({
        url: 'Home/PauseJob',
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
        url: 'Home/ResumeJob',
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

function getAllScheduler() {
    $("#divTb").load('Home/AllScheduler')
}



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