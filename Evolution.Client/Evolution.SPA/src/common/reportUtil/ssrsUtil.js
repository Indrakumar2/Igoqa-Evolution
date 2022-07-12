import { RequestPayload,EvolutionReportsAPIConfig,configuration } from '../../apiConfig/apiConfig';
import IntertekToaster from '../baseComponents/intertekToaster';
import { FetchData, PostData } from '../../services/api/baseApiService';
import { isEmpty, isEmptyReturnDefault, getlocalizeData,parseValdiationMessage,FilterSave, isUndefined,formatToDecimal } from '../../utils/commonUtils';

const localConstant = getlocalizeData();

export const DownloadReportFile = async (params,contentType,fileName,fileExtension,props,reportURL)=> {

    const d = new Date();
    const dformat = `${ d.getHours() }${ d.getMinutes() }${ d.getSeconds() }`;
    const reportFileName = fileName + fileExtension;
    const url = (reportURL ==undefined) ?(EvolutionReportsAPIConfig.reportBaseUrl + EvolutionReportsAPIConfig.EvolutionReportBaseUrl) : reportURL;
    const requestPayload = new RequestPayload(params);
    props.actions.ShowLoader();
    const response = await PostData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            props.actions.HideLoader();
            return false;
        });
    if (response) {
        props.actions.HideLoader();
        const _finalResponse = response;
        if(_finalResponse && _finalResponse.code && (_finalResponse.code == 200 || _finalResponse.code =="1") ){
            const _data = _finalResponse.data ==undefined? _finalResponse.result:_finalResponse.data;
            const binaryString = window.atob(_data);
            const binaryLen = binaryString.length;
            const bytes = new Uint8Array(binaryLen);
            for (let i = 0; i < binaryLen; i++) {
                const ascii = binaryString.charCodeAt(i);
                bytes[i] = ascii;
            }

            const blob = new Blob([ bytes ], { type: contentType });
            if (navigator.appVersion.toString().indexOf('.NET') > 0) {
                window.navigator.msSaveBlob(blob, reportFileName);
            }
            else {
                const link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = reportFileName;
                link.click();
            }
        }
        else{
            if (_finalResponse && _finalResponse.message) {
                IntertekToaster(_finalResponse.message, 'warningToast ssrs_report');
            }
            else{
                IntertekToaster(localConstant.commonConstants.ERROR_MSG, 'dangerToast ssrs_report');
            }
        }
    }
    else {
        IntertekToaster(localConstant.commonConstants.ERROR_MSG, 'dangerToast ssrs_report');
    }
};