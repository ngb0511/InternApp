var currentPage = 1; // Trang hiện tại
var pageSize = 10; // Số lượng dòng mỗi trang
var totalPages = getTotalPages(pageSize);

$(function () {
    $(document).on("ajaxStart", function () {
        $('#loading-icon').show();
    });

    // Ẩn biểu tượng loading khi yêu cầu AJAX kết thúc
    $(document).on("ajaxStop", function () {
        $('#loading-icon').hide();
    });
    document.getElementById("current-page").innerText = "Trang " + currentPage;

    loadData(currentPage, pageSize);
});

function loadData(page, pageSize) {
    // Hiển thị biểu tượng loading khi gửi yêu cầu AJAX
    $.ajax({
        url: "/DummyCode/GetData",
        method: 'GET',
        dataType: 'json',
        data: { page: page, pageSize: pageSize }, // Truyền tham số page và pageSize
        success: function (data) {
            $('#people-table tbody').empty();
            console.log(data);
            // Đổ dữ liệu mới vào bảng
            $.each(data, function (index, dummyCode) {
                //var stt = parseInt(index) + 1;
                var viewLink = '<i onclick="GetData(' + 1 + ', ' + dummyCode.id + ')" style="cursor: pointer;" class="fas fa-eye"></i>';
                var editLink = '<i onclick="GetData(' + 2 + ', ' + dummyCode.id + ')" style="cursor: pointer;" class="fas fa-edit"></i>';
                var deleteLink = '<i onclick="DummyCodeDelete(' + dummyCode.id + ')" style="cursor: pointer;" class="fas fa-trash"></i>';

                $('#people-table tbody').append(
                    '<tr><td>' + dummyCode.material + '</td><td>' + dummyCode.dpName +
                    '</td><td>' + dummyCode.description + '</td><td>' + dummyCode.totalMapping +
                    '</td><td>' + FormatDateTime(dummyCode.createdDate) + '</td><td>' + dummyCode.createdBy +
                    '</td><td>' + viewLink + '</td><td>' + editLink + '</td><td>' + deleteLink + '</td></tr>'
                );
            });
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
        }
    });
}

function FirstPageOnClick () {
    currentPage = 1;
    document.getElementById("current-page").innerText = "Trang " + currentPage;
    loadData(currentPage, pageSize);
};

// Event khi nhấn nút "Trang trước"
function PrevPageOnClick() {
    console.log(currentPage);
    if (currentPage > 1) {
        currentPage--;
        document.getElementById("current-page").innerText = "Trang " + currentPage;
        loadData(currentPage, pageSize);
    }
};

// Event khi nhấn nút "Trang sau"
async function NextPageOnClick() {
    var totalPages = await getTotalPages(pageSize);
    console.log(totalPages);
    if (currentPage < totalPages) {
        currentPage++;
        document.getElementById("current-page").innerText = "Trang " + currentPage;
        loadData(currentPage, pageSize);
    }
}

async function LastPageOnClick() {
    var totalPages = await getTotalPages(pageSize);
    currentPage = totalPages;
    document.getElementById("current-page").innerText = "Trang " + currentPage;
    loadData(currentPage, pageSize);
}

async function getTotalPages(pageSize) {
    try {
        var response = await $.ajax({
            url: "/DummyCode/GetNumTotalPages",
            method: 'GET',
            dataType: 'json',
            data: { pageSize: pageSize }
        });

        return response;
    } catch (error) {
        console.error('Error:', error);
        // Xử lý khi gặp lỗi
        return 0; // hoặc trả về giá trị mặc định khác tùy thuộc vào trường hợp cụ thể của bạn
    }
}

function ItemsPerPageOnChange() {
    var selectedValue = document.getElementById("items-per-page").value;
    pageSize = parseInt(selectedValue); // Cập nhật giá trị pageSize
    currentPage = 1;

    // Gọi hàm loadData với trang hiện tại và pageSize mới
    document.getElementById("current-page").innerText = "Trang " + currentPage;

    
    loadData(currentPage, pageSize);
}

function CreateDummyCode() {
    $("#modalCreate").modal("show");
}

function ExportToExcel() {
    $.ajax({
        type: 'GET',
        url: "/DummyCode/ExportExcel",
        xhrFields: {
            responseType: 'blob' // Đặt kiểu dữ liệu trả về là blob
        },
        success: function (data) {
            var url = window.URL.createObjectURL(data);
            var a = document.createElement('a');
            a.href = url;
            a.download = 'Exported_DummyCode.xlsx';
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        },
        error: function () {
            // Xử lý lỗi
            Swal.fire({
                icon: "error",
                title: "Đã xảy ra lỗi khi xuất Excel",
                showClass: {
                    popup: 'animate__animated animate__fadeInDown'
                },
                hideClass: {
                    popup: 'animate__animated animate__fadeOutUp'
                }
            });
        }
    });
}

function HideModal() {
    $("#modalCreate").modal("hide");
    $("#modalUpdate").modal("hide");
    $("#modalView").modal("hide");
}

function UpdateEvent() {
    var formData = new FormData();
    var material = $("#up_txtMaterial").val();
    var dpName = $("#up_txtDpName").val();
    var description = $("#up_txtDescription").val();
    var totalMapping = $("#up_txtTotalMapping").val();
    var createdDate = $("#up_txtCreatedDate").val();
    var createdBy = $("#up_txtCreatedBy").val();
    var id = $("#up_txtId").val();


    if (material.trim() != "" && dpName.trim() != "" && description.trim() != "") {
        formData.append("Material", material);
        formData.append("DpName", dpName);
        formData.append("Description", description);
        formData.append("TotalMapping", totalMapping);
        formData.append("CreatedDate", createdDate);
        formData.append("CreatedBy", createdBy);
        formData.append("Id", id);
        console.log(formData.get('Id'));
        $.ajax({
            type: 'POST',
            url: "/DummyCode/DummyCodeEdit",
            contentType: false,
            processData: false,
            data: formData,
            success: function (data) {
                if (data.success == false) {
                    Swal.fire({
                        icon: "error",
                        title: data.message,
                        text: "Sửa thất bại",
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
                    $("#txtMaterial").val("");
                    $("#txtDpName").val("");
                    $("#txtDescription").val("");
                    $("#modalUpdate").modal("hide");
                    loadData(currentPage, pageSize);
                    Swal.fire({
                        icon: "success",
                        title: data.message,
                        text: "Sửa thành công",
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
}

function CreateEvent() {
    var formData = new FormData();
    var material = $("#txtMaterial").val();
    var dpName = $("#txtDpName").val();
    var description = $("#txtDescription").val();
    if (material.trim() != "" && dpName.trim() != "" && description.trim() != "") {
        formData.append("Material", material);
        formData.append("DpName", dpName);
        formData.append("Description", description);

        $.ajax({
            type: 'POST',
            url: "/DummyCode/DummyCodeAdd",
            contentType: false,
            processData: false,
            data: formData,
            success: function (data) {
                if (data.success == false) {
                    Swal.fire({
                        icon: "error",
                        title: data.message,
                        text: "Thêm thất bại",
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
                    $("#txtMaterial").val("");
                    $("#txtDpName").val("");
                    $("#txtDescription").val("");
                    $("#modalCreate").modal("hide");
                    loadData(currentPage, pageSize);
                    Swal.fire({
                        icon: "success",
                        title: data.message,
                        text: "Thêm thành công",
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
            },
            error: function (xhr, textStatus, errorThrown) {
                if (xhr.status == 400) {
                    var responseData = xhr.responseJSON;

                    Swal.fire({
                        icon: "error",
                        title: "Thêm thất bại",
                        text: responseData.responseBody,
                        showClass: {
                            popup: 'animate__animated animate__fadeInDown'
                        },
                        hideClass: {
                            popup: 'animate__animated animate__fadeOutUp'
                        }
                    });
                } else {
                    // Handle other error cases
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
}

//Lấy thông tin người dùng để cập nhật
function GetData(actionID, id) {
    console.log(id);
    $.ajax({
        url: "/DummyCode/DummyCodeDetail",
        type: 'GET',
        data: {
            id: id
        },
        success: function (data) {
            var type = '';
            var action = '';
            if (actionID == 2) {
                type = 'up';
                action = 'Update';
            }
            else {
                type = 'view';
                action = 'View';
            }
            $('#' + type + '_txtMaterial').val(data.material);
            $('#' + type + '_txtDpName').val(data.dpName);
            $('#' + type + '_txtDescription').val(data.description);
            $('#' + type + '_txtTotalMapping').val(data.totalMapping);
            $('#' + type + '_txtCreatedDate').val(FormatDateTime(FormatDate(data.createdDate)));
            $('#' + type + '_txtCreatedBy').val(data.createdBy);
            $('#' + type + '_txtId').val(data.id);
            $('#modal' + action).modal("show");
        }
    })
}

function DummyCodeDelete(id) {
    Swal.fire({
        title: 'Confirmation',
        text: 'Are you sure you want to delete this item?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
        customClass: {
            popup: 'animate__animated animate__fadeInDown'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/DummyCode/DummyCodeDelete",
                type: 'POST',
                data: {
                    id: id
                },
                success: function (data) {
                    loadData(currentPage, pageSize);
                    Swal.fire({
                        icon: "success",
                        title: data.message,
                        text: "Xóa thành công",
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
                },
                error: function (xhr, status, error) {
                    // Xử lý lỗi nếu có
                    console.error('Lỗi:', error);
                }
            });
        }
    });
}

function FormatDate(date) {
    var datetime = new Date(date);
    var year = datetime.getFullYear();
    var month = ('0' + (datetime.getMonth() + 1)).slice(-2); // Thêm 0 phía trước nếu cần
    var day = ('0' + datetime.getDate()).slice(-2); // Thêm 0 phía trước nếu cần
    var formattedDate = year + '-' + month + '-' + day;

    return formattedDate
}

function FormatDateTime(dateTime) {
    var date = new Date(dateTime);
    var year = date.getFullYear();
    var month = ('0' + (date.getMonth() + 1)).slice(-2); // Thêm 0 phía trước nếu cần
    var day = ('0' + date.getDate()).slice(-2); // Thêm 0 phía trước nếu cần
    var hours = ('0' + date.getHours()).slice(-2); // Thêm 0 phía trước nếu cần
    var minutes = ('0' + date.getMinutes()).slice(-2); // Thêm 0 phía trước nếu cần
    var seconds = ('0' + date.getSeconds()).slice(-2); // Thêm 0 phía trước nếu cần
    var formattedDateTime = day + '/' + month + '/' + year + ' ' + hours + ':' + minutes;
    return formattedDateTime;
}