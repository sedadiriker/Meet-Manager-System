document.getElementById('DocumentPath').addEventListener('change', function() {
    var fileInput = document.getElementById('DocumentPath');
    var fileInfo = document.getElementById('file-info');

    if (fileInput.files.length > 0) {
        var fileName = fileInput.files[0].name;
        fileInfo.innerText = 'Seçilen dosya: ' + fileName;
    } else {
        fileInfo.innerText = 'Hiç dosya seçilmedi';
    }
});