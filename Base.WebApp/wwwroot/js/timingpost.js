$(document).ready(function () {
    $(document).ajaxStart(function () {
        $('#loading-icon').show();
    });

    // Ẩn biểu tượng loading khi yêu cầu AJAX kết thúc
    $(document).ajaxStop(function () {
        $('#loading-icon').hide();
    });
    // Hàm gửi yêu cầu AJAX        
    // Gọi hàm loadData để tải dữ liệu khi trang được load
    loadData();
});

function loadData() {
    // Hiển thị biểu tượng loading khi gửi yêu cầu AJAX
    $.ajax({
        url: '/TimingPost/GetData', // Đường dẫn tới action GetData
        method: 'GET',
        success: function (data) {
            $('#people-table tbody').empty();

            // Đổ dữ liệu mới vào bảng
            $.each(data, function (index, timing) {
                var stt = parseInt(index) + 1;
                var editLink = '@Html.ActionLink("Edit", "Edit", new { /* id=item.PrimaryKey */})';
                $('#people-table tbody')
                    .append('<tr><td>' + stt + '</td><td>' + timing.customer + '</td><td>' + timing.postName
                        + '</td><td>' + timing.postStart + '</td><td>' + timing.postEnd
                        + '</td><td>' + timing.createdDate + '</td><td>' + timing.createdBy
                        + '</td><td>' + editLink
                        + '</td></tr>');
            });
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
        }
    });
}

function CreateTiming() {
    $("#modalCreate").modal("show");
}

function HideModal() {
    $("#modalCreate").modal("hide");
}

document.getElementById("txtPostStart").addEventListener("change", function () {
    var startDate = new Date(document.getElementById("txtPostStart").value);
    var endDate = new Date(document.getElementById("txtPostEnd").value);

    if (startDate >= endDate) {
        alert("Ngày bắt đầu phải trước ngày kết thúc!");
        document.getElementById("txtPostStart").value = ""; // Xóa giá trị ngày bắt đầu
    }
});

document.getElementById("txtPostEnd").addEventListener("change", function () {
    var startDate = new Date(document.getElementById("txtPostStart").value);
    var endDate = new Date(document.getElementById("txtPostEnd").value);

    if (startDate >= endDate) {
        alert("Ngày kết thúc phải sau ngày bất đầu!");
        document.getElementById("txtPostEnd").value = ""; // Xóa giá trị ngày bắt đầu
    }
});

$('#btnCreate').click(function () {
    var formData = new FormData();
    var customer = $("#txtCustomer").val();
    var postName = $("#txtPostName").val();
    var postStart = $("#txtPostStart").val();
    var postEnd = $("#txtPostEnd").val();
    if (customer.trim() != "" && postName.trim() != "" && postStart.trim() != "" && postEnd.trim() != "") {
        formData.append("Customer", customer);
        formData.append("PostName", postName);
        formData.append("PostStart", postStart);
        formData.append("PostEnd", postEnd);
        $.ajax({
            type: 'POST',
            url: '@Url.Action("Create", "TimingPost")',
            contentType: false,
            processData: false,
            data: formData,
            success: function (data) {
                if (data.success == false) {
                    Swal.fire({
                        icon: "error",
                        title: data.message,
                        showClass: {
                            popup: 'animate__animated animate__fadeInDown'
                        },
                        hideClass: {
                            popup: 'animate__animated animate__fadeOutUp'
                        }
                    }).then(function (result) {
                        if (result.value) {
                        }
                    })
                } else {
                    $("#txtCustomer").val("");
                    $("#txtPostName").val("");
                    $("#txtPostStart").val("");
                    $("#txtPostEnd").val("");
                    $("#modalCreate").modal("hide");
                    loadData();
                    Swal.fire({
                        icon: "success",
                        title: data.message,
                        showClass: {
                            popup: 'animate__animated animate__fadeInDown'
                        },
                        hideClass: {
                            popup: 'animate__animated animate__fadeOutUp'
                        }
                    }).then(function (result) {
                        if (result.value) {
                            dataTable.ajax.reload(null, false);
                        }
                    })
                }
            }
        })
    } else {
        Swal.fire({
            icon: "error",
            title: "Vui lòng điền đầy đủ thông tin",
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        }).then(function (result) {
            if (result.value) {
            }
        })
    }
})