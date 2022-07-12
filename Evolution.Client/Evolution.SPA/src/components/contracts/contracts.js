import React, { Fragment } from 'react';
import ReactGrid from '../../common/baseComponents/reactAgGrid';
import { getlocalizeData } from '../../../src/utils/commonUtils';
import LabelwithValue from '../../common/baseComponents/customLabelwithValue';
import CustomInput from '../../common/baseComponents/inputControlls';
const localConstant = getlocalizeData();
const commonContracts = (props) => {

    const StatusOptions = [
        { value: localConstant.contract.ALL },
        { value: localConstant.contract.OPEN },
        { value: localConstant.contract.ClOSED }
    ];
    return (
        <Fragment>

            <div className="genralDetailContainer customCard mt-0">
                <div className="row mb-0">
                    <div className="col s2 " >
                        <p className="bold">Contracts</p>

                    </div>
                    <div className="offset-s6 col s4">
                        <LabelwithValue
                            className=" col s6 pr-2 mt-2 m6 right-align"
                            label={localConstant.companyDetails.Contracts.CONTRACT_STATUS}
                        />
                        <CustomInput
                            hasLabel={false}
                            divClassName='col s4 pl-0 m6'
                            type='select'
                            className="browser-default"
                            optionsList={StatusOptions}
                            optionName='value'
                            optionValue="value"
                            onSelectChange={props.handleChangeStatus}
                            defaultValue="Open"
                          //  value={ props.selectedcontractStatus}
                        />
                    </div>
                </div>
                <div className="customCard">
                    <ReactGrid gridRowData={props.ContractDetail} gridColData={props.headData} />
                </div>
            </div>
        </Fragment>
    );
};
export default commonContracts;