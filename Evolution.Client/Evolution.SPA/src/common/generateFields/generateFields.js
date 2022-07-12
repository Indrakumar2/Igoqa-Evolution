import React, { Fragment } from 'react';
import PropTypes from 'prop-types'; 
import arrayUtil from '../../utils/arrayUtil';
import { GetComponent } from '../generateFields/findComponent';
// import ExportToCSV from '../../components/applicationComponents/exportToCSV';

const AssignData = (properties, data, setValues) => {
    const { dataKey, name, type } = properties;
    if (dataKey in data) {
        if (type === 'select') {
            properties.optionsList = data[dataKey];
        }
        if(type === 'grid'){
            properties.gridRowData = data[dataKey];
        }
        if(type === 'date'){
            properties.selectedDate = data[dataKey];
        }
    }
    if (name in setValues) {
        if ('defaultValue' in properties) properties.defaultValue = setValues[name]?setValues[name]:'';
        else if('value' in properties) properties.value = setValues[name]?setValues[name]:'';
        else if('selectedDate' in properties)  properties.selectedDate = setValues[name]?setValues[name]:'';
    }
};

//Currently Not in use, Need to do few changes on assigning props as per requirement
const ProcessChildFields = (props) => (props.child.subComponents && props.child.subComponents.length > 0) ?
    <FormChildFields
        subChilds={props.child.subComponents}
        onFieldChange={props.onFieldChange}
    />
    : null;
   
//Added text in the else condition
const DefineComponent = (props) => {
    const { child,data, setValues,csvRef } = props,
          { properties = {} } = child;
    switch (child.component) {
        case 'Button':
            return <button id={properties.id} type={properties.type} onClick={properties.onClick} className={properties.className} >{child.text}</button>;
        case 'ButtonWithDiv':
            return <div className={'col mb-1 ' + properties.divClassName + ' ' + properties.colSize} > <button id={properties.id} type={properties.type} onClick={properties.onClick} className={properties.className} >{child.text}</button></div>;
        // case 'ExportToCSV':
        //     return <ExportToCSV csvExportClick={properties.csvExportClick}
        //         csvRef={csvRef}
        //         buttonClass={properties.buttonClass}
        //         csvClass={properties.csvClass}
        //         filename={properties.filename}
        //         data={data && data.csvData ? data.csvData : []}
        //         header={data && data.csvHeader ? data.csvHeader : []}
            // data={[]}
            // header={[]}
            // />;
    default:
        const Component = GetComponent(child.component);
        AssignData(properties,data, setValues);
        const hasChilds = child.subComponents && child.subComponents.length > 0 ?           
                <Component fields={child.subComponents}
                    text={child.text}
                    {...properties}
                    idx={props.idx}
                /> 
            :
            <Component
                text={child.text}
                {...properties}
                id={properties.id}
            />;
            
        return null !== Component ?
            hasChilds :
            <Component
                text={child.text}
                {...properties}
                id={properties.id}
            />;
    }
};
//Currently Not in use, Need to do few changes on assigning props as per requirement
export const FormChildFields = (childProps) => {
    return (
        childProps.subChilds.map((field, index) => {
            const { subComponents } = field;
            const Component = GetComponent(field.component);
            if (!Component) {
                return null;
            }
            return (<Fragment key={index}>
                {
                    (subComponents && subComponents.length > 0) ?
                        <Component text={field.text}
                            {...field.properties}>
                            <FormChildFields
                                subChilds={field.subComponents}
                                onFieldChange={childProps.onFieldChange}
                            />
                        </Component>
                        :
                        <Component
                            {...field.properties}
                            key={field.properties.id}
                        />
                }
            </Fragment>);
        })
    );
};

export const ProcessSchema = (props) => {
    const { childFields, data, setValues,csvRef } = props;
    let GenerateFields = [];
    arrayUtil.isArray(childFields) ? GenerateFields = childFields : GenerateFields.push(childFields);
    return (
        <div className="row mb-0">
            {
                GenerateFields.map((child, index) => {
                    return (
                        <DefineComponent key={index} child={child} idx={index + 1} data={data} setValues={setValues} csvRef={csvRef}/>
                    );
                })
            }
        </div>
    );
};

DefineComponent.propTypes = {
    child: PropTypes.object.isRequired,
    onFieldChange: PropTypes.func,
    setValues:PropTypes.object
};

DefineComponent.defaultProps = {
    child: {
        subComponents: []
    },
    setValues: {}
};

ProcessChildFields.propTypes = {
    child: PropTypes.object.isRequired,
    onFieldChange: PropTypes.func
};

ProcessChildFields.defaultProps = {
    child: {
        subComponents: []
    }
};

ProcessSchema.propTypes = {
    childFields: PropTypes.array.isRequired,
    onFieldChange: PropTypes.func,
    setValues:PropTypes.object
};

ProcessSchema.defaultProps = {
    childFields: [],
    setValues: {}
};
