import MaterializeComponent from 'materialize-css';
import IntertekToaster from '../../baseComponents/intertekToaster';
export const SuccessAlert = (data) => () => {
    IntertekToaster('Company data updated successfully','successToast CompanyDataUpdateSuccess');
};

export const FailureAlert = (data) => () => {
    IntertekToaster('Company update failed','dangerToast CompanyDataUpdateFailed');
};

export const WarningAlert = (data) => () => {
    IntertekToaster('Company data updated partially','warningToast CompanyDataUpdatePartially');
};

export const ValidationAlert = (data) => () => {
    IntertekToaster(data,'warningToast CompanyDataUpdatePartially');
};