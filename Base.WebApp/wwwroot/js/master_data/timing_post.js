// Định nghĩa các component VueJS
Vue.component('modal-create', {
    props: ['show'],
    template: `
            <div class="modal" v-if="show">
                <!-- Nội dung modal tạo -->
            </div>
            `
});

Vue.component('modal-update', {
    props: ['show'],
    template: `
            <div class="modal" v-if="show">
                <!-- Nội dung modal cập nhật -->
            </div>
            `
});

Vue.component('modal-import', {
    props: ['show'],
    template: `
            <div class="modal" v-if="show">
                <!-- Nội dung modal nhập Excel -->
            </div>
            `
});

Vue.component('loading', {
    props: ['show'],
    template: `
            <div class="loading" v-if="show">
                <!-- Nội dung biểu tượng loading -->
            </div>
            `
});

new Vue({
    el: '#app',
    data: {
        searchInput: '',
        pageSize: 10,
        pageIndex: 1,
        pageTotal: 1,
        showModalCreate: false,
        showModalUpdate: false,
        showModalImport: false,
        isLoading: false
    },
    methods: {
        createTiming() {
            this.showModalCreate = true;
        },
        closeModalCreate() {
            this.showModalCreate = false;
        },
        importTiming() {
            this.showModalImport = true;
        },
        closeModalImport() {
            this.showModalImport = false;
        },
        exportTiming() {
            // Thực hiện chức năng xuất Excel
        },
        searchTable() {
            // Thực hiện chức năng tìm kiếm
        },
        changePageSize() {
            // Thực hiện chức năng thay đổi kích thước trang
        },
        firstPageOnClick() {
            // Thực hiện chức năng chuyển đến trang đầu tiên
        },
        prevPageOnClick() {
            // Thực hiện chức năng chuyển đến trang trước đó
        },
        nextPageOnClick() {
            // Thực hiện chức năng chuyển đến trang kế tiếp
        },
        lastPageOnClick() {
            // Thực hiện chức năng chuyển đến trang cuối cùng
        }
    }
});