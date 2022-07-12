import { viewAllRightsModules } from '../constants/securityConstant';
import { isEmpty } from './commonUtils';

export const CheckPermission = (permissions, activities) => {
    let result = false;
    if (permissions == undefined || permissions == null || permissions.length <= 0)
        result = true;
    else if (activities == undefined || activities == null || activities.length <= 0)
        result = false;
    else {
        permissions.forEach((value, index, array) => {
            if (result == false) {
                const searcResult = activities.filter(x => x.activity == value && x.hasPermission == true);
                result = (searcResult != undefined && searcResult != null && searcResult.length > 0);
            }
        });
    }
    return result;
};

export const ButtonShowHide = (editMode,viewMode,createMode) => {
    let isInEditMode= editMode;
    let isInViewMode=viewMode;
    const isInCreateMode=createMode;
    if(isInCreateMode){
      isInEditMode=false;
      isInViewMode=true;
    }
    else if(isInViewMode){
      isInEditMode=false;
      isInViewMode=false;
    }
    else if(isInEditMode)
    {
      isInViewMode=true;
    }
    const mode=[ isInViewMode ,isInEditMode ] ;
    return mode;
};

export const moduleViewAllRights = (module, isViewAllRights) => {
    if(module){
        if(viewAllRightsModules.includes(module) && isViewAllRights)
            return true;
        else
            return false;
    }
    return false;
};

/**
 * 
 * @param {* Module Name } module 
 * @param {* Result of view all rights API call } viewAllRightsCompanies 
 */
export const moduleViewAllRights_Modified = (module, viewAllRightsCompanies) => {
    if(isEmpty(viewAllRightsCompanies))
        return false;
    if(module){
        const moduleData = viewAllRightsCompanies.filter(x => x.moduleName === module);
        if(viewAllRightsModules.includes(module) && moduleData.length > 0)
            return moduleData[0].viewAllCompanies && moduleData[0].viewAllCompanies.length > 0 ? true : false;
        else
            return false;
    }
    return false;
};