// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ScrollableDetailsList } from '@aksio/cratis-fluentui';
import { IColumn } from '@fluentui/react';

const columns: IColumn[] = [
    {
        key: 'name',
        name: 'Name',
        fieldName: 'name',
        minWidth: 200,
        maxWidth: 200
    },
    {
        key: 'id',
        name: 'Identifier',
        fieldName: 'id',
        minWidth: 200
    }
];

export const Tenants = () => {
    const items: any[] = [];

    return (
        <ScrollableDetailsList columns={columns} items={items}/>
    );
};
