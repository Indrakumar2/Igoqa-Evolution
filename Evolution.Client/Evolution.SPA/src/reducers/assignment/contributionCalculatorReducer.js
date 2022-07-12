import { assignmentsActionTypes } from '../../constants/actionTypes';
import { isEmptyReturnDefault } from '../../utils/commonUtils';
import objectUtil from '../../utils/objectUtil';

export const ContributionCalculatorReducer = (state, action) => {
    const { type, data } = action;
    let newContributions = [], calculatedData = {};
    switch (type) {
        case assignmentsActionTypes.ADD_REVENUE_DATA:
        case assignmentsActionTypes.ADD_COST_DATA:
            newContributions = isEmptyReturnDefault(objectUtil.cloneDeep(state.assignmentDetail.AssignmentContributionCalculators));
            if(newContributions.length === 0 ){
                newContributions[0] = {};
            }
            newContributions[0].assignmentContributionRevenueCosts = [ ...newContributions[0].assignmentContributionRevenueCosts, data ];
            calculatedData = contributionCalculation(newContributions);
            newContributions[0] = { ...newContributions[0], ...calculatedData };
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentContributionCalculators: newContributions
                },
                isContrinutionCalculatorModified:true,
                isbtnDisable: false 
            };
            return state;
        case assignmentsActionTypes.UPDATE_REVENUE_DATA:
        case assignmentsActionTypes.UPDATE_COST_DATA:
            newContributions = isEmptyReturnDefault(objectUtil.cloneDeep(state.assignmentDetail.AssignmentContributionCalculators));
            if(newContributions.length === 0 ){
                newContributions[0] = {};
            }
            newContributions[0].assignmentContributionRevenueCosts = updateObjectInArray(newContributions[0].assignmentContributionRevenueCosts, data);
            calculatedData = contributionCalculation(newContributions);
            newContributions[0] = { ...newContributions[0], ...calculatedData };
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentContributionCalculators: newContributions
                },
                isContrinutionCalculatorModified:true,
                isbtnDisable: false
            };
            return state;
        case assignmentsActionTypes.DELETE_REVENUE_DATA:
        calculatedData = contributionCalculation(data);
        data[0]  = { ...data[0], ...calculatedData };    
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentContributionCalculators: data
                },
                isContrinutionCalculatorModified:true,
                isbtnDisable: false
            };
            return state;
        case assignmentsActionTypes.DELETE_COST_DATA:
            calculatedData = contributionCalculation(data);
            data[0]  = { ...data[0], ...calculatedData };    
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentContributionCalculators: data
                },
                isContrinutionCalculatorModified:true,
                isbtnDisable: false
            };
            return state;
        case assignmentsActionTypes.ADD_DEFAULT_CONTRIBUTION_DATA:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentContributionCalculators: [ {
                        "assignmentContCalculationId": null,
                        "assignmentContCalculationUniqueId": data.assignmentContCalculationUniqueId,
                        "assignmentId": data.assignmentId,
                        "totalContributionValue": 0,
                        "contractHolderPercentage": data.contractHolderPercentage,
                        "operatingCompanyPercentage": data.operatingCompanyPercentage,
                        "countryCompanyPercentage": data.countryCompanyPercentage,
                        "totalContributionModdyPercent":0,
                        "contractHolderValue": 0,
                        "operatingCompanyValue": 0,
                        "countryCompanyValue": null,
                        "markupPercentage": 0,
                        "recordStatus": "N",
                        "assignmentContributionRevenueCosts": [
                            {
                                "assignmentContributionRevenueCostId": null,
                                "assignmentContributionRevenueCostUniqueId": data.assignmentContributionRevenueCostUniqueId,
                                "assignmentContCalculationId": data.assignmentContCalculationUniqueId,
                                "type": "A",
                                "value": null,
                                "description": "Bill Rate",
                                "recordStatus": "N",
                                "createdBy": null,
                            }
                        ]
                    } ]
                }
            };
            return state;
        case assignmentsActionTypes.UPDATE_CONTRIBUTION_CALCULATOR:
            newContributions = objectUtil.cloneDeep(state.assignmentDetail.AssignmentContributionCalculators);
            newContributions[0].contractHolderPercentage = data.contractHolderPercentage;
            newContributions[0].countryCompanyPercentage = data.countryCompanyPercentage;
            newContributions[0].operatingCompanyPercentage = data.operatingCompanyPercentage;
            if(data.recordStatus && data.recordStatus !== 'N')
                data.recordStatus = 'M';
            else
                data.recordStatus = 'N';
            newContributions[0].recordStatus = data.recordStatus;
            calculatedData = contributionCalculation(newContributions);
            newContributions[0] = { ...newContributions[0], ...calculatedData };
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentContributionCalculators: newContributions
                },
                isContrinutionCalculatorModified:true,
                isbtnDisable: false
            };
            return state;
            case assignmentsActionTypes.RESET_CONTRIBUTION_CALCULATOR:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentContributionCalculators: data
                },
                isContrinutionCalculatorModified:false
            };
            return state;
            case assignmentsActionTypes.SAVED_CONTRIBUTION_CALCULATOR_CHANGES:
            const newData = objectUtil.cloneDeep(state.assignmentDetail.AssignmentContributionCalculators);
            if(newData.length === 0 ){
                newData[0] = {};
            }
            calculatedData = contributionCalculation(newData);
            newData[0] = { ...newData[0], ...calculatedData };
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentContributionCalculators: newData
                },
                contributionCalculationSavedChanges:newData,
                isContrinutionCalculatorModified:false
            };
            return state;
        default:
            return state;
    }
};

function updateObjectInArray(array, action) {
    const actionIndex = array.findIndex(iteratedValue => {
        return iteratedValue[action.checkProperty]
            === action.editedItem[action.checkProperty];
    });
    return array.map((item, index) => {
        if (index !== actionIndex) {
            // This isn't the item we care about - keep it as-is
            return item;
        }

        // Otherwise, this is the one we want - return an updated value
        return {
            ...item,
            ...action.editedItem
        };
    });
}

const sumCalculation = (arrData) => {
    const sum = arrData.map(item => parseFloat(item.value)).reduce((prev, next) => prev + next, 0);
    return isNaN(parseFloat(sum)) ? 0 : parseFloat(sum).toFixed(2);
};

function totalMoodyPercentValue(revenueData, costData) {
    const moodyUSD = sumCalculation(revenueData) - sumCalculation(costData);
    let lastBillrate = 0;
    if (Array.isArray(revenueData)) {
        for (let i = 0; i < revenueData.length; i++) {
            const revenueDesc = revenueData[i].description && revenueData[i].description.trim();
            const revenueValue = revenueData[i].value ? parseFloat(revenueData[i].value) : 0;
            if (revenueDesc.toLowerCase() === ("Bill Rate").toLowerCase()) {
                lastBillrate = isNaN(revenueValue)? 0 : revenueValue;
            }
        }
    }

    let moodyPercent = lastBillrate === 0 ? 0 : (moodyUSD / lastBillrate) * 100;
    moodyPercent = isNaN(moodyPercent) ? '0.00' : moodyPercent;
    return {
        moodyUSD: Number(moodyUSD).toFixed(2), // ITK D-711
        moodyPercent: Number(moodyPercent).toFixed(2)
    };
}; 

function contributionCalculation(contributionData) {
    try {
        const ContributionCalculatorObj = isEmptyReturnDefault(contributionData[0], 'object');
        let contractHolderPercentage = parseFloat(ContributionCalculatorObj.contractHolderPercentage),
            operatingCompanyPercentage = parseFloat(ContributionCalculatorObj.operatingCompanyPercentage),
            countryCompanyPercentage = parseFloat(ContributionCalculatorObj.countryCompanyPercentage);

        contractHolderPercentage = isNaN(contractHolderPercentage) ? 0.0 : contractHolderPercentage;
        operatingCompanyPercentage = isNaN(operatingCompanyPercentage) ? 0.0 : operatingCompanyPercentage;
        countryCompanyPercentage = isNaN(countryCompanyPercentage) ? 0.0 : countryCompanyPercentage;

        // const markupPercentage = contractHolderPercentage + operatingCompanyPercentage + countryCompanyPercentage;

        const assignmentContributionRevenueCosts = isEmptyReturnDefault(ContributionCalculatorObj.assignmentContributionRevenueCosts);
        const revenueData = assignmentContributionRevenueCosts.filter(eachItem => eachItem.type === 'A');
        const costData = assignmentContributionRevenueCosts.filter(eachItem => eachItem.type === 'B');

        const moody = totalMoodyPercentValue(revenueData, costData);

        const operatingCompanyValue = (moody.moodyUSD * (operatingCompanyPercentage / 100));
        const contractHolderValue = (moody.moodyUSD * (contractHolderPercentage / 100));
        const countryCompanyValue = (moody.moodyUSD * (countryCompanyPercentage / 100));

        return {
            operatingCompanyValue,
            contractHolderValue,
            countryCompanyValue,
            markupPercentage: moody.moodyPercent,
            totalContributionValue: moody.moodyUSD,
            totalContributionModdyPercent: moody.moodyPercent,
            contractHolderPercentage,
            operatingCompanyPercentage,
            countryCompanyPercentage
        };
    }
    catch (err) {
        return {
            operatingCompanyValue: 0,
            contractHolderValue: 0,
            countryCompanyValue: 0,
            markupPercentage: 0,
            totalContributionValue: 0,
            totalContributionModdyPercent: 0,
            contractHolderPercentage:0,
            operatingCompanyPercentage:0,
            countryCompanyPercentage:0
        };
    }
}