import MaterializeComponent from 'materialize-css';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { required } from '../../../utils/validator';
export const SuccessAlert = (data, module) => () => {
    IntertekToaster(module + " " + "data updated successfully", 'successToast alertActSuccAlt');
};

export const FailureAlert = (data, module) => () => {
    IntertekToaster(module + " " + "update failed", 'dangerToast alertActFailAlt');
};

export const WarningAlert = (data, module) => () => {
    IntertekToaster(module + " " + "data updated partially", 'warningToast alertActWarnAlt');
};

export const ValidationAlert = (data, module) => () => {
    if(!required(data)){
        IntertekToaster(data, 'warningToast alertActValidAlt');
    }
};

export const DeleteAlert = (data, module) => () => {
    IntertekToaster(module + " " + "data deleted successfully", 'successToast alertActDelAlt');
};

export const CreateAlert = (data, module) => () => {
    IntertekToaster(module + " " + "data created successfully", 'successToast alertActCreatAlt');
};