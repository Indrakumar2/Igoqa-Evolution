import AplicationComponents from '../../componentConfig';

export const GetComponent = (componentName, moduleName) => {
    moduleName = moduleName ? moduleName:'common';
    const component = null;
    const componentsConfig = AplicationComponents[moduleName];
    for (let index = 0, length = componentsConfig.length; index < length; index++) {
        const comp = componentsConfig[index];
        if (componentName === comp.name) {
            return comp.componentType ==='constant'? comp.component[componentName] : comp.component.default;
        }
    }
    return component;
};
