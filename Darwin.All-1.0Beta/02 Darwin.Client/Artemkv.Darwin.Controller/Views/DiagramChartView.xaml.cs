using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Visual.Primitives;
using Artemkv.Darwin.Visual.Connectors;
using System.Windows.Media.Effects;
using Artemkv.Darwin.Visual.Layout;

namespace Artemkv.Darwin.Controller.Views
{
	/// <summary>
	/// Interaction logic for EntityView.xaml
	/// </summary>
	public partial class DiagramChartView : UserControl, IDetailsView
	{
		#region Constants

		private const double ConnectorMargin = 50.00;
		private const double ElementMargin = 50.00;
		private const int GridCellSize = 4;

		#endregion Constants

		#region Private Members

		private List<Primitive> _items = new List<Primitive>();
		private List<Connector> _links = new List<Connector>();

		private Container _selectedItem;
		private Point _dragStartPoint;
		private Rectangle _selectionRectangle;
		private bool _isDragging;
		private Point _rightClickPosition;

		private double _minObjectX = double.NaN;
		private double _minObjectY = double.NaN;
		private double _maxObjectX = double.NaN;
		private double _maxObjectY = double.NaN;

		#endregion Private Members

		#region .Ctor

		public DiagramChartView()
		{
			InitializeComponent();
		}

		#endregion .Ctor

		#region IDetailsView Members

		public PersistentObjectDTO Object
		{
			get { return this.DataContext as PersistentObjectDTO; }
			set
			{
				var diagram = value as DiagramDTO;
				if (diagram == null)
					throw new ArgumentNullException("value");

				ChartCanvas.Width = (double)new LengthConverter().ConvertFrom(String.Format("{0}in", PaperSize.GetWidth(diagram.PaperSize, diagram.IsVertical)));
				ChartCanvas.Height = (double)new LengthConverter().ConvertFrom(String.Format("{0}in", PaperSize.GetHeight(diagram.PaperSize, diagram.IsVertical)));

				this.DataContext = value;

				ReBuild(diagram);
				ReRender();
			}
		}

		#endregion IDetailsView Members

		#region Public Methods

		public void SetItems(List<Primitive> items)
		{
			if (items == null)
				throw new ArgumentNullException("items");

			_items = items;
		}

		public void SetConnections(List<Connector> links)
		{
			if (links == null)
				throw new ArgumentNullException("links");

			_links = links;
		}

		public void ReRender()
		{
			ChartCanvas.Children.Clear();

			_minObjectX = double.NaN;
			_minObjectY = double.NaN;
			_maxObjectX = double.NaN;
			_maxObjectY = double.NaN;

			RenderItems();
			RenderConnections();

			if (IsLoaded)
			{
				AdjustGridSize();
			}
		}

		#endregion Public Methods

		#region Private Methods

		#region Render Methods

		private void RenderItems()
		{
			foreach (Primitive item in _items)
			{
				RenderItem(item);

				if (double.IsNaN(_minObjectX) || _minObjectX > item.X)
				{
					_minObjectX = item.X;
				}
				if (double.IsNaN(_minObjectY) || _minObjectY > item.Y)
				{
					_minObjectY = item.Y;
				}
				if (double.IsNaN(_maxObjectX) || _maxObjectX < item.X + item.Width)
				{
					_maxObjectX = item.X + item.Width;
				}
				if (double.IsNaN(_maxObjectY) || _maxObjectY < item.Y + item.Height)
				{
					_maxObjectY = item.Y + item.Height;
				}
			}
		}

		private void RenderItem(Primitive item)
		{
			TextRectangle textRectangle = item as TextRectangle;
			if (textRectangle != null)
			{
				RenderTextRectangle(textRectangle);
			}

			Container container = item as Container;
			if (container != null)
			{
				RenderContainer(container);
			}
		}

		private void RenderTextRectangle(TextRectangle textRectangle)
		{
			var strokeBrush = new SolidColorBrush(textRectangle.Color);
			var fillBrush = new SolidColorBrush(textRectangle.BgColor);

			var rectangle = new Rectangle()
			{
				Width = textRectangle.Width,
				Height = textRectangle.Height + 1.00,
				Fill = fillBrush,
				Stroke = strokeBrush
			};
			rectangle.SetValue(Canvas.LeftProperty, textRectangle.X);
			rectangle.SetValue(Canvas.TopProperty, textRectangle.Y);
			rectangle.SetValue(Canvas.ZIndexProperty, 2);
			ChartCanvas.Children.Add(rectangle);

			var textBox = new TextBlock()
			{
				Width = textRectangle.Width,
				Height = textRectangle.Height,
				Text = textRectangle.Text,
				Padding = new Thickness(textRectangle.Padding),
				FontSize = textRectangle.EmSize
				// TODO: Text color
			};
			textBox.SetValue(Canvas.LeftProperty, textRectangle.X);
			textBox.SetValue(Canvas.TopProperty, textRectangle.Y);
			textBox.SetValue(Canvas.ZIndexProperty, 2);
			ChartCanvas.Children.Add(textBox);
		}

		private void RenderContainer(Container container)
		{
			// Container itself
			var rectangle = new Rectangle()
			{
				Width = container.Width,
				Height = container.Height,
				Fill = Brushes.White,
				Stroke = Brushes.LightGray,
				Effect = new DropShadowEffect() { Color = Colors.LightGray, ShadowDepth = 8, BlurRadius = 8 }
			};
			rectangle.SetValue(Canvas.LeftProperty, container.X);
			rectangle.SetValue(Canvas.TopProperty, container.Y);
			rectangle.SetValue(Canvas.ZIndexProperty, 0);
			ChartCanvas.Children.Add(rectangle);

			// Inner primitives
			foreach (Primitive item in container.Items)
			{
				RenderItem(item);
			}
		}

		private void RenderConnections()
		{
			var pb = new ConnectionPathsBuilder(ConnectorMargin);
			var paths = pb.GetPathsAligned(pb.GetPaths(_items, _links));
			foreach (var path in paths)
			{
				RenderPath(path);
			}
		}

		private void RenderPath(AlignedConnectionPath path)
		{
			var polyline = new ConnectionPathsBuilder(ConnectorMargin).GetPolylineFromAlignedPath(path);
			polyline.SetValue(Canvas.ZIndexProperty, 1);
			ChartCanvas.Children.Add(polyline);

			var diagram = Object as DiagramDTO;
			if (diagram == null)
				throw new InvalidOperationException();

			RenderCrowFoot(
				polyline.Points[1],
				polyline.Points[0],
				path.ConnectionPath.PrimitiveA,
				path.ConnectionPath.SideA,
				path.ConnectionPath.ConnectorTypeA,
				diagram.ShowModality);
			int lastPoint = polyline.Points.Count - 1;
			RenderCrowFoot(
				polyline.Points[lastPoint - 1],
				polyline.Points[lastPoint],
				path.ConnectionPath.PrimitiveB,
				path.ConnectionPath.SideB,
				path.ConnectionPath.ConnectorTypeB,
				diagram.ShowModality);
		}

		private void RenderCrowFoot(Point p1, Point p2, Primitive primitive, Side side, ConnectorType connectorType, bool showModality)
		{
			const int CrowFootOffset = 20;
			const int CrowFootRadius = 10;

			//           ----|             |primitive
			// p1       /    |      p2     |
			// o--------o----o------o------|
			//         /\    |\            |
			//        /  ----| \           |
			//       /          \
			//      /            x,y Intersect
			//     x,y CrowFoot

			double xDirection = 0.00;
			double yDirection = 0.00;

			double xIntersect;
			double yIntersect;
			double xCrowFoot;
			double yCrowFoot;

			switch (side)
			{
				case Side.Left:
					xDirection = -1.00;
					break;
				case Side.Right:
					xDirection = 1.00;
					break;
				case Side.Top:
					yDirection = -1.00;
					break;
				case Side.Bottom:
					yDirection = 1.00;
					break;
				default:
					throw new InvalidOperationException();
			}

			xIntersect = p2.X + primitive.Width / 2.00 * xDirection;
			xCrowFoot = xIntersect + CrowFootOffset * xDirection;
			yIntersect = p2.Y + primitive.Height / 2.00 * yDirection;
			yCrowFoot = yIntersect + CrowFootOffset * yDirection;

			if (connectorType == ConnectorType.OneOrMany ||
				connectorType == ConnectorType.ZeroOrMany)
			{
				var line1 = new Line()
				{
					X1 = xCrowFoot,
					Y1 = yCrowFoot,
					X2 = xIntersect + CrowFootRadius * yDirection,
					Y2 = yIntersect + CrowFootRadius * xDirection,
					Stroke = Brushes.Black, // TODO hardcode
					StrokeThickness = 2 // TODO hardcode 
				};
				line1.SetValue(Canvas.ZIndexProperty, 1);
				ChartCanvas.Children.Add(line1);

				var line2 = new Line()
				{
					X1 = xCrowFoot,
					Y1 = yCrowFoot,
					X2 = xIntersect - CrowFootRadius * yDirection,
					Y2 = yIntersect - CrowFootRadius * xDirection,
					Stroke = Brushes.Black, // TODO hardcode
					StrokeThickness = 2 // TODO hardcode 
				};
				line2.SetValue(Canvas.ZIndexProperty, 1);
				ChartCanvas.Children.Add(line2);
			}
			else
			{
				var line = new Line()
				{
					X1 = xCrowFoot - CrowFootRadius * yDirection,
					Y1 = yCrowFoot - CrowFootRadius * xDirection,
					X2 = xCrowFoot + CrowFootRadius * yDirection,
					Y2 = yCrowFoot + CrowFootRadius * xDirection,
					Stroke = Brushes.Black, // TODO hardcode
					StrokeThickness = 2 // TODO hardcode 
				};
				line.SetValue(Canvas.ZIndexProperty, 1);
				ChartCanvas.Children.Add(line);
			}

			if (showModality)
			{
				double xModality = xCrowFoot + CrowFootRadius * xDirection;
				double yModality = yCrowFoot + CrowFootRadius * yDirection;

				if (connectorType == ConnectorType.ZeroOrOne ||
					connectorType == ConnectorType.ZeroOrMany)
				{
					var ellipse = new Ellipse()
					{
						Width = CrowFootRadius * 2.00,
						Height = CrowFootRadius * 2.00,
						Stroke = Brushes.Black, // TODO hardcode
						StrokeThickness = 2, // TODO hardcode 
						Fill = Brushes.White // TODO hardcode 
					};
					ellipse.SetValue(Canvas.LeftProperty, xModality - CrowFootRadius);
					ellipse.SetValue(Canvas.TopProperty, yModality - CrowFootRadius);
					ellipse.SetValue(Canvas.ZIndexProperty, 1);
					ChartCanvas.Children.Add(ellipse);
				}
				else
				{
					var line = new Line()
					{
						X1 = xModality - CrowFootRadius * yDirection,
						Y1 = yModality - CrowFootRadius * xDirection,
						X2 = xModality + CrowFootRadius * yDirection,
						Y2 = yModality + CrowFootRadius * xDirection,
						Stroke = Brushes.Black, // TODO hardcode
						StrokeThickness = 2 // TODO hardcode 
					};
					line.SetValue(Canvas.ZIndexProperty, 1);
					ChartCanvas.Children.Add(line);
				}
			}
		}

		#endregion Render Methods

		#region Control Methods

		private void ReBuild(DiagramDTO diagram)
		{
			ChartBuilder cb = new ChartBuilder();
			cb.BuildFromDiagram(diagram);
			if (!diagram.IsAdjusted)
			{
				var arranger = new SimpleArranger();
				arranger.Arrange(cb.Items, cb.Connections);
			}
			SetItems(cb.Items);
			SetConnections(cb.Connections);
		}

		private void AutoArrangeChart()
		{
			var diagram = Object as DiagramDTO;
			if (diagram != null)
			{
				diagram.IsAdjusted = false;

				ReBuild(diagram);
				ReRender();
			}
		}

		private Container GetSelectedContainer(Point p)
		{
			foreach (Primitive item in _items)
			{
				var container = item as Container;
				if (container != null &&
					item.X <= p.X &&
					item.X + item.Width >= p.X &&
					item.Y < p.Y &&
					item.Y + item.Height >= p.Y)
				{
					return container;
				}
			}
			return null;
		}

		private void CreateSelectionRectangle(double x, double y, double width, double height)
		{
			if (_selectionRectangle != null)
			{
				ChartCanvas.Children.Remove(_selectionRectangle);
			}

			_selectionRectangle = new Rectangle()
			{
				Width = width,
				Height = height + 1.00,
				Fill = Brushes.Transparent,
				Stroke = Brushes.DarkOrange,
				StrokeThickness = 3.00
			};
			_selectionRectangle.SetValue(Canvas.LeftProperty, x);
			_selectionRectangle.SetValue(Canvas.TopProperty, y);
			_selectionRectangle.SetValue(Canvas.ZIndexProperty, 4);
			ChartCanvas.Children.Add(_selectionRectangle);

			_selectionRectangle.MouseLeftButtonDown += ChartCanvas_MouseLeftButtonDown;
			_selectionRectangle.MouseLeftButtonUp += ChartCanvas_MouseLeftButtonUp;
		}

		private void AdjustSelectedItem(double dX, double dY)
		{
			_selectedItem.X += dX;
			_selectedItem.Y += dY;

			_selectedItem.X = (double)((int)_selectedItem.X / GridCellSize * GridCellSize);
			_selectedItem.Y = (double)((int)_selectedItem.Y / GridCellSize * GridCellSize);

			var diagram = Object as DiagramDTO;
			if (diagram != null)
			{
				diagram.IsAdjusted = true;
			}

			var diagramEntity = _selectedItem.Object as DiagramEntityDTO;
			if (diagramEntity != null)
			{
				diagramEntity.X += (int)dX;
				diagramEntity.Y += (int)dY;

				diagramEntity.X = diagramEntity.X / GridCellSize * GridCellSize;
				diagramEntity.Y = diagramEntity.Y / GridCellSize * GridCellSize;
			}
		}

		private void StartDragging(Point dragStartPoint)
		{
			_isDragging = true;
			_dragStartPoint = dragStartPoint;
			CreateSelectionRectangle(_selectedItem.X, _selectedItem.Y, _selectedItem.Width, _selectedItem.Height);
		}

		private void EndDragging(Point dragEndPoint)
		{
			_isDragging = false;
			if (_selectedItem != null)
			{
				AdjustSelectedItem(dragEndPoint.X - _dragStartPoint.X, dragEndPoint.Y - _dragStartPoint.Y);
				ReRender();
				CreateSelectionRectangle(_selectedItem.X, _selectedItem.Y, _selectedItem.Width, _selectedItem.Height);
			}
		}

		private void ClearSelectionRectangle()
		{
			if (_selectionRectangle != null)
			{
				ChartCanvas.Children.Remove(_selectionRectangle);
				_selectionRectangle = null;
			}
		}

		private void AdjustGridSize()
		{
			double actualWidth = _maxObjectX - _minObjectX;
			double actualHeight = _maxObjectY - _minObjectY;

			double rightExtraWidth = 0.00;
			if (_maxObjectX > ChartCanvas.Width)
			{
				rightExtraWidth = _maxObjectX - ChartCanvas.Width + ElementMargin;
			}
			double leftExtraWidth = 0.00;
			if (_minObjectX < 0.00)
			{
				leftExtraWidth = Math.Abs(_minObjectX) + ElementMargin;
			}

			double lowerExtraHeight = 0.00;
			if (_maxObjectY > ChartCanvas.Height)
			{
				lowerExtraHeight = _maxObjectY - ChartCanvas.Height + ElementMargin;
			}
			double upperExtraHeight = 0.00;
			if (_minObjectY < 0.00)
			{
				upperExtraHeight = Math.Abs(_minObjectY) + ElementMargin;
			}

			double horisontalPadding = Math.Max(leftExtraWidth, rightExtraWidth);
			double verticalPadding = Math.Max(lowerExtraHeight, upperExtraHeight);
			ChartGrid.Width = horisontalPadding * 2 + ChartCanvas.Width;
			ChartGrid.Height = verticalPadding * 2 + ChartCanvas.Height;

			ChartScroll.ScrollToHorizontalOffset(horisontalPadding);
			ChartScroll.ScrollToVerticalOffset(verticalPadding);
		}

		private void ConnectItems(Container from, Container to)
		{
			var fromEntity = from.Object as DiagramEntityDTO;
			var toEntity = to.Object as DiagramEntityDTO;

			if (fromEntity != null && 
				toEntity != null)
			{
				ServiceLocator serviceLocator = ServiceLocator.GetActive();
				serviceLocator.BasicController.ProcessSaveChanges(continueEditing: true);
				serviceLocator.ModelController.ProcessConnectEntitiesOnDiagram(fromEntity, toEntity);
			}
		}

		#endregion Control Methods

		#endregion Private Methods

		#region Event Handlers

		private void ChartCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if ((Keyboard.Modifiers & ModifierKeys.Shift) > 0)
			{
				if (_selectedItem != null)
				{
					var newSelectedItem = GetSelectedContainer(e.GetPosition(ChartCanvas));
					if (newSelectedItem != null)
					{
						ConnectItems(_selectedItem, newSelectedItem);
					}
				}
			}

			_selectedItem = GetSelectedContainer(e.GetPosition(ChartCanvas));

			if (_selectedItem != null)
			{
				StartDragging(e.GetPosition(ChartCanvas));
			}
			else
			{
				ClearSelectionRectangle();
			}
		}

		private void ChartCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (_isDragging)
			{
				EndDragging(e.GetPosition(ChartCanvas));
			}
		}

		private void ChartCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			if (_isDragging && _selectionRectangle != null)
			{
				var dragEndPoint = e.GetPosition(ChartCanvas);
				CreateSelectionRectangle(
					_selectedItem.X + dragEndPoint.X - _dragStartPoint.X,
					_selectedItem.Y + dragEndPoint.Y - _dragStartPoint.Y,
					_selectedItem.Width,
					_selectedItem.Height);
			}
		}

		private void ChartCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			_rightClickPosition = Mouse.GetPosition(ChartCanvas);

			_selectedItem = GetSelectedContainer(e.GetPosition(ChartCanvas));
			if (_selectedItem != null)
			{
				CreateSelectionRectangle(_selectedItem.X, _selectedItem.Y, _selectedItem.Width, _selectedItem.Height);
			}
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			ChartGrid.MinWidth = this.ActualWidth;
			ChartGrid.MinHeight = this.ActualHeight;
			AdjustGridSize();
		}

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			ChartGrid.Width = double.NaN;
			ChartGrid.Height = double.NaN;
			ChartGrid.MinWidth = this.ActualWidth;
			ChartGrid.MinHeight = this.ActualHeight;
			AdjustGridSize();
		}

		#endregion Event Handlers

		#region Menu Event Handlers

		private void AutoArrange_MenuItem_Click(object sender, RoutedEventArgs e)
		{
			AutoArrangeChart();
		}

		private void Properties_MenuItem_Click(object sender, RoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.BasicController.ProcessSaveChanges(continueEditing: true);
			serviceLocator.ModelController.ProcessEditDiagramObject(Object);
		}

		private void NewEntity_MenuItem_Click(object sender, RoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.BasicController.ProcessSaveChanges(continueEditing: true);
			serviceLocator.ModelController.ProcessAddNewEntityOnDiagram(Object as DiagramDTO, _rightClickPosition);
		}

		private void EditEntity_MenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (_selectedItem != null && _selectedItem.Object != null)
			{
				var diagramEntity = _selectedItem.Object as DiagramEntityDTO;
				if (diagramEntity != null && diagramEntity.Entity != null)
				{
					ServiceLocator serviceLocator = ServiceLocator.GetActive();
					serviceLocator.BasicController.ProcessSaveChanges(continueEditing: true);
					serviceLocator.ModelController.ProcessEditEntity(diagramEntity.Entity);
					serviceLocator.BasicController.ProcessProjectTreeRefresh();
				}
			}
		}

		#endregion Menu Event Handlers
	}
}
