/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/organization/settings/elasticsearch');

export interface ISetElasticSearchSettings {
    url?: string;
    apiKey?: string;
}

export class SetElasticSearchSettingsValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        url: new Validator(),
        apiKey: new Validator(),
    };
}

export class SetElasticSearchSettings extends Command<ISetElasticSearchSettings> implements ISetElasticSearchSettings {
    readonly route: string = '/api/organization/settings/elasticsearch';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetElasticSearchSettingsValidator();

    private _url!: string;
    private _apiKey!: string;

    get requestArguments(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'url',
            'apiKey',
        ];
    }

    get url(): string {
        return this._url;
    }

    set url(value: string) {
        this._url = value;
        this.propertyChanged('url');
    }
    get apiKey(): string {
        return this._apiKey;
    }

    set apiKey(value: string) {
        this._apiKey = value;
        this.propertyChanged('apiKey');
    }

    static use(initialValues?: ISetElasticSearchSettings): [SetElasticSearchSettings, SetCommandValues<ISetElasticSearchSettings>] {
        return useCommand<SetElasticSearchSettings, ISetElasticSearchSettings>(SetElasticSearchSettings, initialValues);
    }
}
