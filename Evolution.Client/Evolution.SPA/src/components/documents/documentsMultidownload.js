import { commonAPIConfig } from '../../apiConfig/apiConfig';
import { uploadedDocumentCheck } from '../../utils/commonUtils';

export const MultipleDownload = (selectedRecords) => {
    const isFileUploaded = uploadedDocumentCheck(selectedRecords); //To validate requested doc is uploaded or not
    if (isFileUploaded) {
        selectedRecords.map(selectedRecord => {
            const isIE = /*@cc_on!@*/false || !!document.documentMode;
            if (isIE) {
                const downloadDocumentDataUrl = commonAPIConfig.baseUrl + commonAPIConfig.documents + '/' + selectedRecord.moduleCode + commonAPIConfig.download + selectedRecord.documentUniqueName;
                const link = document.createElement("a");
                link.download = selectedRecord.documentName;
                link.href = downloadDocumentDataUrl;
                link.target = "_blank";
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            }
            else {
                const ifrm = document.createElement("IFRAME");
                ifrm.setAttribute("style", "display:none;");
                ifrm.setAttribute("src", commonAPIConfig.baseUrl + commonAPIConfig.documents + '/' + selectedRecord.moduleCode + commonAPIConfig.download + selectedRecord.documentUniqueName);
                ifrm.style.width = 0 + "px";
                ifrm.style.height = 0 + "px";
                document.body.appendChild(ifrm);
            }
        });
    }
};