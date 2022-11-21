/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/settings/pulumi');

export interface ISetPulumiSettings {
    organization?: string;
    accessToken?: string;
}

export class SetPulumiSettingsValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        organization: new Validator(),
        accessToken: new Validator(),
    };
}

export class SetPulumiSettings extends Command<ISetPulumiSettings> implements ISetPulumiSettings {
    readonly route: string = '/api/settings/pulumi';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetPulumiSettingsValidator();

    private _organization!: string;
    private _accessToken!: string;

    get requestArguments(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'organization',
            'accessToken',
        ];
    }

    get organization(): string {
        return this._organization;
    }

    set organization(value: string) {
        this._organization = value;
        this.propertyChanged('organization');
    }
    get accessToken(): string {
        return this._accessToken;
    }

    set accessToken(value: string) {
        this._accessToken = value;
        this.propertyChanged('accessToken');
    }

    static use(initialValues?: ISetPulumiSettings): [SetPulumiSettings, SetCommandValues<ISetPulumiSettings>, ClearCommandValues] {
        return useCommand<SetPulumiSettings, ISetPulumiSettings>(SetPulumiSettings, initialValues);
    }
}
