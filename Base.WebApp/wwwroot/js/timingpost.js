var pageIndex = 1;
var totalPages = 0;
var pageSize = 10;

$(document).ready(function () {

    loadData(1);

    GetTotalPage();

    document.getElementById("txtPostStart").addEventListener("change", function () {
        var startDate = new Date(document.getElementById("txtPostStart").value);
        var endDate = new Date(document.getElementById("txtPostEnd").value);

        if (startDate >= endDate) {
            Swal.fire({
                icon: "error",
                title: "Ngày bắt đầu phải trước ngày kết thúc!",
                showClass: {
                    popup: 'animate__animated animate__fadeInDown'
                },
                hideClass: {
                    popup: 'animate__animated animate__fadeOutUp'
                }
            });
            document.getElementById("txtPostStart").value = ""; // Xóa giá trị ngày bắt đầu
        }
    });

    document.getElementById("txtPostEnd").addEventListener("change", function () {
        var startDate = new Date(document.getElementById("txtPostStart").value);
        var endDate = new Date(document.getElementById("txtPostEnd").value);

        if (startDate >= endDate) {
            Swal.fire({
                icon: "error",
                title: "Ngày kết thúc phải sau ngày bắt đầu!",
                showClass: {
                    popup: 'animate__animated animate__fadeInDown'
                },
                hideClass: {
                    popup: 'animate__animated animate__fadeOutUp'
                }
            });
            document.getElementById("txtPostEnd").value = ""; // Xóa giá trị ngày bắt đầu
        }
    });

    document.getElementById("up_txtPostStart").addEventListener("change", function () {
        var startDate = new Date(document.getElementById("up_txtPostStart").value);
        var endDate = new Date(document.getElementById("up_txtPostEnd").value);

        if (startDate >= endDate) {
            Swal.fire({
                icon: "error",
                title: "Ngày bắt đầu phải trước ngày kết thúc!",
                showClass: {
                    popup: 'animate__animated animate__fadeInDown'
                },
                hideClass: {
                    popup: 'animate__animated animate__fadeOutUp'
                }
            });
            document.getElementById("up_txtPostStart").value = ""; // Xóa giá trị ngày bắt đầu
        }
    });

    document.getElementById("up_txtPostEnd").addEventListener("change", function () {
        var startDate = new Date(document.getElementById("up_txtPostStart").value);
        var endDate = new Date(document.getElementById("up_txtPostEnd").value);

        if (startDate >= endDate) {
            Swal.fire({
                icon: "error",
                title: "Ngày kết thúc phải sau ngày bắt đầu!",
                showClass: {
                    popup: 'animate__animated animate__fadeInDown'
                },
                hideClass: {
                    popup: 'animate__animated animate__fadeOutUp'
                }
            });
            document.getElementById("up_txtPostEnd").value = ""; // Xóa giá trị ngày bắt đầu
        }
    });

});

/*function toggleScrollButtons() {
    const table = document.getElementById('people-table');
    const isTableOverflowing = table.scrollHeight > table.clientHeight;
    const scrollUpBtn = document.getElementById('scroll-up-btn');
    const scrollDownBtn = document.getElementById('scroll-down-btn');
    scrollUpBtn.style.display = isTableOverflowing ? 'block' : 'none';
    scrollDownBtn.style.display = isTableOverflowing ? 'block' : 'none';
}*/

function changePageSize() {
    document.getElementById("pageIndex").value = 1;
    pageIndex = 1;
    loadData(pageIndex); // Gọi loadData với pageIndex = 1 khi thay đổi kích thước trang
}

function changePage() {
    pageIndex = parseInt(document.getElementById("pageIndex").value);
    loadData(pageIndex); // Gọi loadData với pageIndex mới khi thay đổi trang
}

function loadData(pageIndex) {
    document.getElementById("pageIndex").innerText = "Trang " + pageIndex;
    $(document).ajaxStart(function () {
        $('#loading').show();
    });
    // Ẩn biểu tượng loading khi yêu cầu AJAX kết thúc
    $(document).ajaxStop(function () {
        $('#loading').hide();
    });
    $('#data-container').empty();
    pageSize = parseInt($('#pageSize').val());
    GetTotalPage();
    console.log(pageIndex, pageSize, totalPages);
    // Hiển thị biểu tượng loading khi gửi yêu cầu AJAX
    var baseurl = "https://localhost:7083/api/TimingPost/Paging?pageIndex=" + pageIndex + "&pageSize=" + pageSize;
    $.ajax({
        url: baseurl, // Đường dẫn tới action GetData
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            const tbody = document.querySelector('#people-table tbody');
            tbody.innerHTML = '';
            if (data.length === 0) {
                $('#people-table tbody').append('<tr><td colspan="8" class="text-center">Không có dữ liệu để hiển thị</td></tr>');
            } else {
                // Đổ dữ liệu mới vào bảng
                $.each(data, function (index, timing) {
                    var editLink = '<button onclick="GetData(' + timing.id + ')" class="btn btn-trash"><i class="fas fa-edit" ></i></button >';
                    var deleteLink = '<button onclick="Delete(' + timing.id + ')" class="btn btn-trash"><i class="fas fa-trash" ></i></button >';
                    console.log(timing.index, pageSize);
                    tbody.innerHTML += `
                                    <tr>
                                        <td>${timing.index}</td>
                                        <td>${timing.customer}</td>
                                        <td>${timing.postName}</td>
                                        <td>${FormatDateDisplay(timing.postStart) }</td>
                                        <td>${FormatDateDisplay(timing.postEnd) }</td>
                                        <td>${FormatDateTime(timing.createdDate) }</td>
                                        <td>${timing.createdByName}</td>
                                        <td>${editLink} ${deleteLink}</td>
                                    </tr>
                                `;
                });
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
        }
    });
}

function CreateTiming() {
    $("#modalCreate").modal("show");
}

function ImportTiming() {
    $("#modalImport").modal("show");
}

function HideModal() {
    $("#modalCreate").modal("hide");
    $("#modalUpdate").modal("hide");
    $("#modalImport").modal("hide");
}

function Create() {
    var formData = new FormData();
    var customer = $("#txtCustomer").val();
    var postName = $("#txtPostName").val();
    var postStart = $("#txtPostStart").val();
    var postEnd = $("#txtPostEnd").val();
    if (customer.trim() !="" && postName.trim() != "" && postStart.trim() != "" && postEnd.trim() != "") {
        formData.append("Customer", customer);
        formData.append("PostName", postName);
        formData.append("PostStart", postStart);
        formData.append("PostEnd", postEnd);

        $.ajax({
            type: 'POST',
            url: '/TimingPost/Create',
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
                    loadData(pageIndex);
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
function Update() {
    var formData = new FormData();
    var customer = $("#up_txtCustomer").val();
    var postName = $("#up_txtPostName").val();
    var postStart = $("#up_txtPostStart").val();
    var postEnd = $("#up_txtPostEnd").val();
    var id = $("#up_txtId").val();


    if (customer.trim() != "" && postName.trim() != "" && postStart.trim() != "" && postEnd.trim() != "") {
        formData.append("Customer", customer);
        formData.append("PostName", postName);
        formData.append("PostStart", postStart);
        formData.append("PostEnd", postEnd);
        formData.append("Id", id);
        $.ajax({
            type: 'POST',
            url: '/TimingPost/Edit',
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
                    $("#up_txtCustomer").val("");
                    $("#up_txtPostName").val("");
                    $("#up_txtPostStart").val("");
                    $("#up_txtPostEnd").val("");
                    $("#modalUpdate").modal("hide");
                    loadData(pageIndex);
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

//Lấy thông tin người dùng để cập nhật
function GetData(id) {
    $('#loading').hide();
    $.ajax({
        url: '/TimingPost/GetById',
        type: 'get',
        data: {
            id: id
        },
        success: function (result) {
            if (result.data.success = true) {
                $('#up_txtCustomer').val(result.data.customer);
                $('#up_txtPostName').val(result.data.postName);
                $('#up_txtPostStart').val(FormatDate(result.data.postStart));
                $('#up_txtPostEnd').val(FormatDate(result.data.postEnd));
                $('#up_txtCreatedDate').val(FormatDateTime(result.data.createdDate));
                $('#up_txtCreatedBy').val(result.data.createdByName);
                $('#up_txtId').val(result.data.id);
                $('#modalUpdate').modal("show");
            }
        }
    })
}

function Delete(id) {
    $.ajax({
        url: 'https://localhost:7083/api/TimingPost?id=' + id, // Đường dẫn tới action GetData
        type: 'DELETE',
        success: function (data) {
            loadData(pageIndex);
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

                }
            })
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
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

function FormatDateDisplay(date) {
    var datetime = new Date(date);
    var year = datetime.getFullYear();
    var month = ('0' + (datetime.getMonth() + 1)).slice(-2); // Thêm 0 phía trước nếu cần
    var day = ('0' + datetime.getDate()).slice(-2); // Thêm 0 phía trước nếu cần
    var formattedDate = day + '/' + month + '/' + year;

    return formattedDate
}

function FormatDateTime(dateTime) {
    var date = new Date(dateTime);
    var year = date.getFullYear();
    var month = ('0' + (date.getMonth() + 1)).slice(-2); // Thêm 0 phía trước nếu cần
    var day = ('0' + date.getDate()).slice(-2); // Thêm 0 phía trước nếu cần
    var hours = ('0' + date.getHours()).slice(-2); // Thêm 0 phía trước nếu cần
    var minutes = ('0' + date.getMinutes()).slice(-2); // Thêm 0 phía trước nếu cần
    var formattedDateTime = day + '/' + month + '/' + year + ' ' + hours + ':' + minutes;
    return formattedDateTime;
}

function Import() {
    var fileInput = document.getElementById('fileInput');
    if (fileInput.files.length > 0) {
        var file = fileInput.files[0];
        var formData = new FormData();
        formData.append('file', file);
        $(document).off('ajaxStart');
        Swal.fire({
            title: 'Loading...',
            allowOutsideClick: false,
            showCancelButton: false,
            showConfirmButton: false,
            willOpen: () => {
                $('#loading').hide();
                Swal.showLoading();
            },
            didClose: () => {
                Swal.hideLoading();
                $(document).on('ajaxStart', function () {
                    $('#loading').show();
                });
                $('#loading').hide();
            }
        });
        $(document).ajaxStop(function () {
            $('#loading').hide();
        });
        $.ajax({
            url: 'https://localhost:7083/api/TimingPost/ImportTimingPostFromExcel',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.success == true) {
                    $("#modalImport").modal("hide");
                    $("#fileInput").val(null);
                    loadData(1);
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

                        }
                    })
                } else {
                    Swal.fire({
                        icon: "error",
                        title: data.message,
                        showClass: {
                            popup: 'animate__animated animate__fadeInDown'
                        },
                        hideClass: {
                            popup: 'animate__animated animate__fadeOutUp'
                        }
                    });
                }
            },
            error: function (xhr, status, error) {
                console.error('Lỗi khi gửi file:', error);
            }
        });
    } else {
        Swal.fire({
            icon: "error",
            title: "Vui lòng chọn file",
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });
    }
}

function Export() {
    $.ajax({
        url: 'https://localhost:7083/api/TimingPost/ExportExcelFile',
        type: 'GET',
        xhrFields: {
            responseType: 'blob' // Đặt kiểu dữ liệu trả về là blob
        },
        success: function (data) {
            Swal.close();
            var url = window.URL.createObjectURL(data);
            var a = document.createElement('a');
            a.href = url;
            a.download = 'Exported_TimingPost.xlsx';
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        },
        error: function (xhr, status, error) {
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

function PrevPageOnClick() {
    pageIndex = pageIndex - 1;
    loadData(pageIndex)
}

function NextPageOnClick() {
    pageIndex = pageIndex + 1;
    loadData(pageIndex);
}

function FirstPageOnClick() {
    pageIndex = 1;
    loadData(pageIndex);
}

function LastPageOnClick() {
    pageIndex = totalPages;
    loadData(pageIndex);
}

function ButtonStatus(pageIndex) {
    var buttonPrev = document.getElementById("prev-page");
    var buttonNext = document.getElementById("next-page"); 
    var buttonLast = document.getElementById("last-page"); 
    var buttonFirst = document.getElementById("first-page"); 
    if (pageIndex > 1) {
        buttonPrev.disabled = false;
        buttonFirst.disabled = false;
    } else {
        buttonPrev.disabled = true;
        buttonFirst.disabled = true;
    }
    if (totalPages > 1) {
        $('#footer').css('display', 'block');
    } else {
        $('#footer').css('display', 'none');
    }
    if (pageIndex >= totalPages) {
        buttonNext.disabled = true;
        buttonLast.disabled = true;
    } else {
        buttonNext.disabled = false;
        buttonLast.disabled = false;
    }
}

function GetTotalPage() {
    var baseurl = "https://localhost:7083/api/TimingPost/GetAll";
    $.ajax({
        url: baseurl, // Đường dẫn tới action GetData
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            var countItem = data.length;
            totalPages = Math.ceil(countItem / pageSize);
            document.getElementById("pageTotal").innerText = "/ " + totalPages;
            ButtonStatus(pageIndex);
            if (pageIndex > totalPages) {
                pageIndex = totalPages;
                loadData(pageIndex);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
        }
    });
}
