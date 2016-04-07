$(function () {
    getAllScheduler();
    $("#btnRefresh").click(getAllScheduler);
    $("#btnRevise").click(revise);
    $("#btnReload").click(reloadScheduler);
    $("#btnShow").click(showData);

    $("#btnUpload").click(uploadZip);
    $("#btnAddOK").click(saveScheduler);

    $("#btnAdd").click(function () {
        $("#hidSchedulerId").val(0);
        $("#schTitle").text("添加任务");
        $("#modalScheduler").modal({
            show: true,
            keyboard: false

        });
    });


    $("#divTb").on("click", "button[cmdStart]", startScheduler);


    $("#divTb").on("click", "button[cmdStop]", stopScheduler);

    $("#divTb").on("click", "button[cmdkill]", killProcess);

    $("#divTb").on("click", "button[cmdUpload]", openUploadModal);



    $("#divTb").on("click", "button[cmdPauseJob]", pauseJob);


    $("#divTb").on("click", "button[cmdResumeJob]", resumeJob);

    $("#divTb").on("click", "button[cmdEdit]", loadScheduler);


});

$.ajaxSetup({
    error: function (data) {
        showWarn("提示", data);
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


function saveScheduler() {
    $("#form2").ajaxSubmit({
        url: "Home/SaveScheduler",
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

function loadScheduler() {
    var self = $(this);
    var schId = self.parents("tr").attr("schid");
    $("#schTitle").text("编辑任务");
    $.ajax({
        url: "Home/GetScheduler",
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
        url: "Home/GetAllSchedulerData",
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