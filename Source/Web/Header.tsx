// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Dropdown, IDropdownOption } from '@fluentui/react';
import { AllOrganizations } from 'API/organizations/AllOrganizations';

export const Header = () => {

    const [organizations] = AllOrganizations.use();


    const options: IDropdownOption[] = organizations.data.map(_ => {
        return {
            key: _.id,
            text: _.name
        };
    });

    return (
        <div>
            <Dropdown options={options}/>

        </div>
    );
};
