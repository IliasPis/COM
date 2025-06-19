mergeInto(LibraryManager.library, {
    WebGLScreenshotDownload: function (base64ImagePtr, fileNamePtr) {
        const base64Image = UTF8ToString(base64ImagePtr);
        const fileName = UTF8ToString(fileNamePtr);

        // Convert the Base64 image into a Blob
        const binary = atob(base64Image);
        const array = [];
        for (let i = 0; i < binary.length; i++) {
            array.push(binary.charCodeAt(i));
        }
        const blob = new Blob([new Uint8Array(array)], { type: 'image/png' });

        // Create a link element and trigger download
        const link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
});
