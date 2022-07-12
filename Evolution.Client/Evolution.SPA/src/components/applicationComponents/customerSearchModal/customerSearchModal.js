import React, { Component, Fragment } from 'react';
import Modal from '../../../common/baseComponents/modal';
import CustomInput from '../../../common/baseComponents/inputControlls';
import ReactGrid from '../../../common/baseComponents/reactAgGrid';
import { getlocalizeData } from '../../../utils/commonUtils';
import HeaderData from './headerData.json';

const localConstant = getlocalizeData();

class CustomerSearchModal extends Component {
    render() {
        const { isShowModal, onValueChange, optionsList, onSelectChange, gridRowData, cancelAction, submitAction } = this.props;
        return (
            <Modal buttons={[ { name: 'Cancel', action: cancelAction, showbtn: true }, { name: 'Submit', action: submitAction, showbtn: true } ]}
                isShowModal={isShowModal}>
                <Fragment >
                    <div className="row mb-0">
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.contract.NAME}
                            type='text'
                            optionName='name'
                            optionValue="name"
                            colSize='s6'
                            name="customerName"
                            inputClass="customInputs"
                            onValueChange={onValueChange}
                        />
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.contract.COUNTRY}
                            type='select'
                            colSize='s6'
                            inputClass="customInputs"
                            optionName='name'
                            optionValue="name"
                            name="operatingCountry"
                            optionsList={optionsList}
                            onSelectChange={onSelectChange}
                        />
                    </div>
                </Fragment>
                <ReactGrid gridRowData={gridRowData} gridColData={HeaderData} onRef={ref => { this.child = ref; }} />
            </Modal>
        );
    }
}

export default CustomerSearchModal;