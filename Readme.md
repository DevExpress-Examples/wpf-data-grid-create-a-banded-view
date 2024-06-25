<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128649072/22.2.2%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E4625)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# WPF Data Grid - Create a Banded View

This example creates a multiple row banded view that consists of the following bands:

* The **Model Details** band includes two child bands (**Model Name** and **Modification**).
* The **Performance Attributes** band contains child columns arranged into three rows.
* The **Description** band is overlayed by its child column (the [OverlayHeaderByChildren](https://docs.devexpress.com/WPF/DevExpress.Xpf.Grid.BandBase.OverlayHeaderByChildren) property is set to `true`).

![image](https://user-images.githubusercontent.com/65009440/209684455-3c7c02fc-b662-40fe-9486-7b438b5e06af.png)

## Files to Review

* [MainWindow.xaml](./CS/MainWindow.xaml) (VB: [MainWindow.xaml](./VB/MainWindow.xaml))
* [MainWindow.xaml.cs](./CS/MainWindow.xaml.cs) (VB: [MainWindow.xaml](./VB/MainWindow.xaml))

## Documentation

* [Bands](https://docs.devexpress.com/WPF/15660/controls-and-libraries/data-grid/grid-view-data-layout/bands)
* [Band Separators](https://docs.devexpress.com/WPF/120139/controls-and-libraries/data-grid/grid-view-data-layout/bands/band-separators)
* [Bands Panel](https://docs.devexpress.com/WPF/114396/controls-and-libraries/data-grid/visual-elements/common-elements/bands-panel)
* [Bind the Grid to Bands Specified in ViewModel](https://docs.devexpress.com/WPF/117249/controls-and-libraries/data-grid/examples/mvvm-enhancements/how-to-bind-the-grid-to-bands-specified-in-viewmodel)

## More Examples

* [Bind the WPF GridControl to a Collection of Bands Specified in ViewModel](https://github.com/DevExpress-Examples/how-to-generate-bands-based-on-a-collection-in-a-viewmodel-e5217)
* [WPF Data Grid - Customize Column and Band Separators](https://github.com/DevExpress-Examples/how-to-draw-custom-separators-for-gridcolumns-and-gridcontrolbands-t192318)
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=wpf-data-grid-create-a-banded-view&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=wpf-data-grid-create-a-banded-view&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
