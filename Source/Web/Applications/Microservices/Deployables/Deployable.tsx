// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Pivot, PivotItem, Stack } from '@fluentui/react';

export const Deployable = () => {
    return (
        <Stack style={{ height: '100%' }}>
            <Stack.Item>
                <Pivot>
                    <PivotItem headerText="General" itemIcon="Settings">
                    </PivotItem>
                    <PivotItem headerText="Environment" itemIcon="Variable">
                    </PivotItem>
                    <PivotItem headerText="Secrets" itemIcon="Encryption">
                    </PivotItem>
                </Pivot>
            </Stack.Item>
        </Stack>
    );
};
