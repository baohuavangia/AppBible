window.editor = {
    init: function (id) {
        $('#' + id).summernote({
            placeholder: 'Nhập nội dung...',
            tabsize: 2,
            height: 250
        });
    },
    getContent: function (id) {
        return $('#' + id).summernote('code');
    },
    setContent: function (id, html) {
        $('#' + id).summernote('code', html);
    }
};
