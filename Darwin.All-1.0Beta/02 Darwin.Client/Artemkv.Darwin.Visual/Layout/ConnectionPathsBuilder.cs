using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Visual.Primitives;
using Artemkv.Darwin.Visual.Connectors;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace Artemkv.Darwin.Visual.Layout
{
	public class ConnectionPathsBuilder
	{
		#region Auxiliary Classes

		private struct Segment
		{
			public int X1;
			public int Y1;
			public int X2;
			public int Y2;

			public Segment(int x1, int y1, int x2, int y2)
			{
				X1 = x1;
				Y1 = y1;
				X2 = x2;
				Y2 = y2;
			}
		}

		private class MySortedList
		{
			private List<KeyValuePair<int, ConnectionPath>> _innerList = new List<KeyValuePair<int, ConnectionPath>>();

			public void Add(int key, ConnectionPath value)
			{
				_innerList.Add(new KeyValuePair<int, ConnectionPath>(key, value));
				_innerList.Sort((x, y) => x.Key.CompareTo(y.Key));
			}

			public int Count
			{
				get
				{
					return _innerList.Count;
				}
			}

			public int IndexOfValue(ConnectionPath value)
			{
				for (int i = 0; i < _innerList.Count; i++)
				{
					if (_innerList[i].Value == value)
					{
						return i;
					}
				}

				throw new InvalidOperationException();
			}
		}

		private class Connectivity
		{
			public Dictionary<Side, MySortedList> LinksPerSide = new Dictionary<Side, MySortedList>()
			{
				{ Side.Left, new MySortedList() },
				{ Side.Right, new MySortedList() },
				{ Side.Top, new MySortedList() },
				{ Side.Bottom, new MySortedList() }
			};
		}

		#endregion Auxiliary Classes

		#region Class Members

		private Dictionary<Primitive, int> _selfReferenceKeys = new Dictionary<Primitive, int>();
		private int _selfReferenceCounter = 0;

		#endregion Class Members

		#region .Ctors

		public ConnectionPathsBuilder(double connectorMargin)
		{
			this.ConnectorMargin = connectorMargin;
		}

		#endregion .Ctors

		#region Public Properties

		public double ConnectorMargin { get; set; }

		#endregion Public Properties

		#region Public Methods

		public List<ConnectionPath> GetPaths(List<Primitive> items, List<Connector> links)
		{
			var paths = new List<ConnectionPath>();
			var linkPaths = new Dictionary<Connector, List<ConnectionPath>>();
			foreach (var link in links)
			{
				linkPaths.Add(link, GetPossiblePathsForLink(link));
				foreach (var path in linkPaths[link])
				{
					if (path.PrimitiveA == link.From)
					{
						path.ConnectorTypeA = link.FromConnectorType;
						path.ConnectorTypeB = link.ToConnectorType;
					}
					else
					{
						path.ConnectorTypeB = link.FromConnectorType;
						path.ConnectorTypeA = link.ToConnectorType;
					}
				}
			}

			FilterPaths(linkPaths);

			foreach (var pathList in linkPaths.Values)
			{
				foreach (var path in pathList)
				{
					paths.Add(path);
				}
			}

			GetPathsAligned(paths);

			return paths;
		}

		public List<AlignedConnectionPath> GetPathsAligned(List<ConnectionPath> paths)
		{
			List<AlignedConnectionPath> alignedPaths = new List<AlignedConnectionPath>();

			Dictionary<Primitive, Connectivity> connectivityPerPrimitive = new Dictionary<Primitive,Connectivity>();

			foreach (var path in paths)
			{
				if (!connectivityPerPrimitive.ContainsKey(path.PrimitiveA))
				{
					connectivityPerPrimitive.Add(path.PrimitiveA, new Connectivity());
				}
				if (!connectivityPerPrimitive.ContainsKey(path.PrimitiveB))
				{
					connectivityPerPrimitive.Add(path.PrimitiveB, new Connectivity());
				}
				if (path.IsSelfReference)
				{
					if (!_selfReferenceKeys.ContainsKey(path.PrimitiveA))
					{
						_selfReferenceKeys.Add(path.PrimitiveA, 1);
					}
					else
					{
						_selfReferenceKeys[path.PrimitiveA] = _selfReferenceKeys[path.PrimitiveA] + 1;
					}
					connectivityPerPrimitive[path.PrimitiveA].LinksPerSide[Side.Right].Add(_selfReferenceKeys[path.PrimitiveA], path);
					connectivityPerPrimitive[path.PrimitiveB].LinksPerSide[Side.Top].Add(Int32.MaxValue -_selfReferenceKeys[path.PrimitiveA], path);
				}
				else
				{
					switch (path.SideA)
					{
						case Side.Left:
							connectivityPerPrimitive[path.PrimitiveA].LinksPerSide[Side.Left].Add((int)path.PrimitiveB.Y, path);
							break;
						case Side.Right:
							connectivityPerPrimitive[path.PrimitiveA].LinksPerSide[Side.Right].Add((int)path.PrimitiveB.Y, path);
							break;
						case Side.Top:
							connectivityPerPrimitive[path.PrimitiveA].LinksPerSide[Side.Top].Add((int)path.PrimitiveB.X, path);
							break;
						case Side.Bottom:
							connectivityPerPrimitive[path.PrimitiveA].LinksPerSide[Side.Bottom].Add((int)path.PrimitiveB.X, path);
							break;
						default:
							throw new InvalidOperationException();
					}
					switch (path.SideB)
					{
						case Side.Left:
							connectivityPerPrimitive[path.PrimitiveB].LinksPerSide[Side.Left].Add((int)path.PrimitiveA.Y, path);
							break;
						case Side.Right:
							connectivityPerPrimitive[path.PrimitiveB].LinksPerSide[Side.Right].Add((int)path.PrimitiveA.Y, path);
							break;
						case Side.Top:
							connectivityPerPrimitive[path.PrimitiveB].LinksPerSide[Side.Top].Add((int)path.PrimitiveA.X, path);
							break;
						case Side.Bottom:
							connectivityPerPrimitive[path.PrimitiveB].LinksPerSide[Side.Bottom].Add((int)path.PrimitiveA.X, path);
							break;
						default:
							throw new InvalidOperationException();
					}
				}
			}

			foreach (var path in paths)
			{
				alignedPaths.Add(new AlignedConnectionPath()
				{
					ConnectionPath = path,
					PathEndAtSideAIndex = connectivityPerPrimitive[path.PrimitiveA].LinksPerSide[path.SideA].IndexOfValue(path),
					PathEndAtSideATotal = connectivityPerPrimitive[path.PrimitiveA].LinksPerSide[path.SideA].Count,
					PathEndAtSideBIndex = connectivityPerPrimitive[path.PrimitiveB].LinksPerSide[path.SideB].IndexOfValue(path),
					PathEndAtSideBTotal = connectivityPerPrimitive[path.PrimitiveB].LinksPerSide[path.SideB].Count
				});
			}

			return alignedPaths;
		}

		public Polyline GetPolylineFromPath(ConnectionPath path)
		{
			var p = new Polyline()
			{
				Stroke = Brushes.Black, // TODO hardcode
				StrokeThickness = 2 // TODO hardcode
			};

			Point p1 = GetPrimitiveCenter(path.PrimitiveA);
			Point p2;
			switch (path.SideA)
			{
				case Side.Left:
					p2 = GetPrimitiveLeftConnector(path.PrimitiveA);
					break;
				case Side.Right:
					p2 = GetPrimitiveRightConnector(path.PrimitiveA);
					break;
				case Side.Top:
					p2 = GetPrimitiveUpperConnector(path.PrimitiveA);
					break;
				case Side.Bottom:
					p2 = GetPrimitiveLowerConnector(path.PrimitiveA);
					break;
				default:
					throw new InvalidOperationException();
			}

			Point p4;
			if (path.IsSelfReference)
			{
				p4 = GetPrimitiveUpperConnector(path.PrimitiveA);
			}
			else
			{
				p4 = GetPrimitiveCenter(path.PrimitiveB);
			}
			Point p3;
			switch (path.SideA)
			{
				case Side.Left:
				case Side.Right:
					p3 = new Point()
					{
						X = p2.X,
						Y = p4.Y
					};
					break;
				case Side.Top:
				case Side.Bottom:
					p3 = new Point()
					{
						X = p4.X,
						Y = p2.Y
					};
					break;
				default:
					throw new InvalidOperationException();
			}

			var points = new PointCollection();
			points.Add(p1);
			points.Add(p2);
			points.Add(p3);
			points.Add(p4);
			if (path.IsSelfReference)
			{
				points.Add(GetPrimitiveCenter(path.PrimitiveB));
			}
			p.Points = points;

			return p;
		}

		public Polyline GetPolylineFromAlignedPath(AlignedConnectionPath alignedPath)
		{
			double dx1 = 0.00;
			double dy1 = 0.00;
			if (alignedPath.ConnectionPath.SideA == Side.Left ||
				alignedPath.ConnectionPath.SideA == Side.Right)
			{
				dy1 = alignedPath.ConnectionPath.PrimitiveA.Y +
					alignedPath.ConnectionPath.PrimitiveA.Height / (double)(alignedPath.PathEndAtSideATotal + 1) * (double)(alignedPath.PathEndAtSideAIndex + 1);
				dy1 -= GetPrimitiveCenter(alignedPath.ConnectionPath.PrimitiveA).Y;
			}
			else
			{
				dx1 = alignedPath.ConnectionPath.PrimitiveA.X +
					alignedPath.ConnectionPath.PrimitiveA.Width / (double)(alignedPath.PathEndAtSideATotal + 1) * (double)(alignedPath.PathEndAtSideAIndex + 1);
				dx1 -= GetPrimitiveCenter(alignedPath.ConnectionPath.PrimitiveA).X;
			}
			double dx2 = 0.00;
			double dy2 = 0.00;
			if (alignedPath.ConnectionPath.SideB == Side.Left ||
				alignedPath.ConnectionPath.SideB == Side.Right)
			{
				dy2 = alignedPath.ConnectionPath.PrimitiveB.Y +
					alignedPath.ConnectionPath.PrimitiveB.Height / (double)(alignedPath.PathEndAtSideBTotal + 1) * (double)(alignedPath.PathEndAtSideBIndex + 1);
				dy2 -= GetPrimitiveCenter(alignedPath.ConnectionPath.PrimitiveB).Y;
			}
			else
			{
				dx2 = alignedPath.ConnectionPath.PrimitiveB.X +
					alignedPath.ConnectionPath.PrimitiveB.Width / (double)(alignedPath.PathEndAtSideBTotal + 1) * (double)(alignedPath.PathEndAtSideBIndex + 1);
				dx2 -= GetPrimitiveCenter(alignedPath.ConnectionPath.PrimitiveB).X;
			}

			var p = GetPolylineFromPath(alignedPath.ConnectionPath);

			var points = new PointCollection();
			if (alignedPath.ConnectionPath.IsSelfReference)
			{
				double selfRefRadius = ConnectorMargin * alignedPath.PathEndAtSideAIndex;

				Point p1 = new Point(p.Points[0].X + dx1, p.Points[0].Y + dy1);
				points.Add(p1);
				Point p2 = new Point(p.Points[1].X + dx1 + selfRefRadius, p.Points[1].Y + dy1);
				points.Add(p2);
				Point p3 = new Point(p.Points[2].X + selfRefRadius, p.Points[2].Y - selfRefRadius);
				points.Add(p3);
				Point p4 = new Point(p.Points[3].X + dx2, p.Points[3].Y + dy2 - selfRefRadius);
				points.Add(p4);
				Point p5 = new Point(p.Points[4].X + dx2, p.Points[4].Y + dy2);
				points.Add(p5);
			}
			else
			{
				Point p1 = new Point(p.Points[0].X + dx1, p.Points[0].Y + dy1);
				points.Add(p1);
				Point p2 = new Point(p.Points[1].X + dx1, p.Points[1].Y + dy1);
				points.Add(p2);
				Point p3 = new Point(p.Points[2].X + dx2, p.Points[2].Y + dy2);
				points.Add(p3);
				Point p4 = new Point(p.Points[3].X + dx2, p.Points[3].Y + dy2);
				points.Add(p4);
			}

			var pAligned = new Polyline()
			{
				Stroke = Brushes.Black, // TODO hardcode
				StrokeThickness = 2 // TODO hardcode
			};

			pAligned.Points = points;
			return pAligned;
		}

		#endregion Public Methods

		#region Private Methods

		private List<ConnectionPath> GetPossiblePathsForLink(Connector link)
		{
			var paths = new List<ConnectionPath>();

			if (link.From.Id == link.To.Id)
			{
				paths.Add(new ConnectionPath()
				{
					PrimitiveA = link.From,
					SideA = Side.Right,
					PrimitiveB = link.To,
					SideB = Side.Top
				});
			}
			else
			{
				paths.AddRange(GetPrimaryPathsFromLeftSide(link.From, link.To));
				paths.AddRange(GetPrimaryPathsFromRightSide(link.From, link.To));

				paths.AddRange(GetPrimaryPathsFromUpperSide(link.From, link.To));
				paths.AddRange(GetPrimaryPathsFromLowerSide(link.From, link.To));

				paths.AddRange(GetPrimaryPathsFromLeftSide(link.To, link.From));
				paths.AddRange(GetPrimaryPathsFromRightSide(link.To, link.From));

				paths.AddRange(GetPrimaryPathsFromUpperSide(link.To, link.From));
				paths.AddRange(GetPrimaryPathsFromLowerSide(link.To, link.From));

				if (paths.Count == 0)
				{
					paths.AddRange(GetSecondaryPathsFromLeftSide(link.From, link.To));
					paths.AddRange(GetSecondaryPathsFromRightSide(link.From, link.To));

					paths.AddRange(GetSecondaryPathsFromUpperSide(link.From, link.To));
					paths.AddRange(GetSecondaryPathsFromLowerSide(link.From, link.To));

					paths.AddRange(GetSecondaryPathsFromLeftSide(link.To, link.From));
					paths.AddRange(GetSecondaryPathsFromRightSide(link.To, link.From));

					paths.AddRange(GetSecondaryPathsFromUpperSide(link.To, link.From));
					paths.AddRange(GetSecondaryPathsFromLowerSide(link.To, link.From));
				}
			}

			return paths;
		}

		private List<ConnectionPath> GetPrimaryPathsFromLeftSide(Primitive a, Primitive b)
		{
			var paths = new List<ConnectionPath>();

			double leftA = a.X - ConnectorMargin;
			double rightB = b.X + b.Width + ConnectorMargin;

			if (leftA >= rightB)
			{
				paths.Add(new ConnectionPath()
				{
					PrimitiveA = a,
					SideA = Side.Left,
					PrimitiveB = b,
					SideB = Side.Right
				});
			}

			return paths;
		}

		private List<ConnectionPath> GetPrimaryPathsFromRightSide(Primitive a, Primitive b)
		{
			var paths = new List<ConnectionPath>();

			double rightA = a.X + a.Width + ConnectorMargin;
			double leftB = b.X - ConnectorMargin;

			if (rightA <= leftB)
			{
				paths.Add(new ConnectionPath()
				{
					PrimitiveA = a,
					SideA = Side.Right,
					PrimitiveB = b,
					SideB = Side.Left
				});
			}

			return paths;
		}

		private List<ConnectionPath> GetPrimaryPathsFromUpperSide(Primitive a, Primitive b)
		{
			var paths = new List<ConnectionPath>();

			double upperA = a.Y - ConnectorMargin;
			double lowerB = b.Y + b.Height + ConnectorMargin;

			if (upperA >= lowerB)
			{
				paths.Add(new ConnectionPath()
				{
					PrimitiveA = a,
					SideA = Side.Top,
					PrimitiveB = b,
					SideB = Side.Bottom
				});
			}

			return paths;
		}

		private List<ConnectionPath> GetPrimaryPathsFromLowerSide(Primitive a, Primitive b)
		{
			var paths = new List<ConnectionPath>();

			double lowerA = a.Y + a.Height + ConnectorMargin;
			double upperB = b.Y - ConnectorMargin;

			if (lowerA <= upperB)
			{
				paths.Add(new ConnectionPath()
				{
					PrimitiveA = a,
					SideA = Side.Bottom,
					PrimitiveB = b,
					SideB = Side.Top
				});
			}

			return paths;
		}

		private List<ConnectionPath> GetSecondaryPathsFromLeftSide(Primitive a, Primitive b)
		{
			var paths = new List<ConnectionPath>();

			double leftA = a.X - ConnectorMargin;
			double leftB = b.X - ConnectorMargin;

			if (leftA <= leftB)
			{
				paths.Add(new ConnectionPath()
				{
					PrimitiveA = a,
					SideA = Side.Left,
					PrimitiveB = b,
					SideB = Side.Left
				});
			}

			return paths;
		}

		private List<ConnectionPath> GetSecondaryPathsFromRightSide(Primitive a, Primitive b)
		{
			var paths = new List<ConnectionPath>();

			double rightA = a.X + a.Width + ConnectorMargin;
			double rightB = b.X + b.Width + ConnectorMargin;

			if (rightA >= rightB)
			{
				paths.Add(new ConnectionPath()
				{
					PrimitiveA = a,
					SideA = Side.Right,
					PrimitiveB = b,
					SideB = Side.Right
				});
			}

			return paths;
		}

		private List<ConnectionPath> GetSecondaryPathsFromUpperSide(Primitive a, Primitive b)
		{
			var paths = new List<ConnectionPath>();

			double upperA = a.Y - ConnectorMargin;
			double upperB = b.Y - ConnectorMargin;

			if (upperA <= upperB)
			{
				paths.Add(new ConnectionPath()
				{
					PrimitiveA = a,
					SideA = Side.Top,
					PrimitiveB = b,
					SideB = Side.Top
				});
			}

			return paths;
		}

		private List<ConnectionPath> GetSecondaryPathsFromLowerSide(Primitive a, Primitive b)
		{
			var paths = new List<ConnectionPath>();

			double lowerA = a.Y + a.Height + ConnectorMargin;
			double lowerB = b.Y + b.Height + ConnectorMargin;

			if (lowerA >= lowerB)
			{
				paths.Add(new ConnectionPath()
				{
					PrimitiveA = a,
					SideA = Side.Bottom,
					PrimitiveB = b,
					SideB = Side.Bottom
				});
			}

			return paths;
		}

		private Point GetPrimitiveCenter(Primitive a)
		{
			return new Point()
			{
				X = a.X + a.Width / 2.00,
				Y = a.Y + a.Height / 2.00
			};
		}

		private Point GetPrimitiveLeftConnector(Primitive a)
		{
			return new Point()
			{
				X = a.X - ConnectorMargin,
				Y = a.Y + a.Height / 2.00
			};
		}

		private Point GetPrimitiveRightConnector(Primitive a)
		{
			return new Point()
			{
				X = a.X + a.Width + ConnectorMargin,
				Y = a.Y + a.Height / 2.00
			};
		}

		private Point GetPrimitiveUpperConnector(Primitive a)
		{
			return new Point()
			{
				X = a.X + a.Width / 2.00,
				Y = a.Y - ConnectorMargin
			};
		}

		private Point GetPrimitiveLowerConnector(Primitive a)
		{
			return new Point()
			{
				X = a.X + a.Width / 2.00,
				Y = a.Y + a.Height + ConnectorMargin
			};
		}

		private void FilterPaths(Dictionary<Connector, List<ConnectionPath>> linkPaths)
		{
			bool isFinal;
			do
			{
				isFinal = true;
				var intersections = GetIntersectionsPerEachPath(linkPaths);

				// Choose paths with minimal intersections
				foreach (var pathList in linkPaths.Values)
				{
					if (pathList.Count <= 1) // TODO: what if 0?
					{
						continue;
					}
					isFinal = false;

					ConnectionPath maxPath = null;
					int maxIntersections = 0;
					foreach (var path in pathList)
					{
						if (intersections[path] >= maxIntersections)
						{
							maxIntersections = intersections[path];
							maxPath = path;
						}
					}
					pathList.Remove(maxPath);
				}
			} while (!isFinal);
		}

		private Dictionary<ConnectionPath, int> GetIntersectionsPerEachPath(Dictionary<Connector, List<ConnectionPath>> linkPaths)
		{
			var intersections = new Dictionary<ConnectionPath, int>();

			foreach (var link in linkPaths.Keys)
			{
				var pathList = linkPaths[link];
				foreach (var path in pathList)
				{
					intersections.Add(path, GetIntersectionsPerPath(path, linkPaths, link));
				}
			}
	
			return intersections;
		}

		private int GetIntersectionsPerPath(ConnectionPath currentPath, Dictionary<Connector, List<ConnectionPath>> linkPaths, Connector currentLink)
		{
			int intersectionsTotal = 0;
			foreach (var link in linkPaths.Keys)
			{
				if (link == currentLink)
				{
					continue;
				}

				var pathList = linkPaths[link];
				foreach (var path in pathList)
				{
					intersectionsTotal += GetIntersectionsBetweenTwoPaths(currentPath, path);
				}
			}
			return intersectionsTotal;
		}

		private int GetIntersectionsBetweenTwoPaths(ConnectionPath pathA, ConnectionPath pathB)
		{
			int intersectionsTotal = 0;

			var lineA = GetPolylineFromPath(pathA);
			Segment[] segmentsA = new Segment[]
			{
				new Segment((int)lineA.Points[0].X, (int)lineA.Points[0].Y, (int)lineA.Points[1].X, (int)lineA.Points[1].Y),
				new Segment((int)lineA.Points[1].X, (int)lineA.Points[1].Y, (int)lineA.Points[2].X, (int)lineA.Points[2].Y),
				new Segment((int)lineA.Points[2].X, (int)lineA.Points[2].Y, (int)lineA.Points[3].X, (int)lineA.Points[3].Y)
			};

			var lineB = GetPolylineFromPath(pathB);
			Segment[] segmentsB = new Segment[]
			{
				new Segment((int)lineB.Points[0].X, (int)lineB.Points[0].Y, (int)lineB.Points[1].X, (int)lineB.Points[1].Y),
				new Segment((int)lineB.Points[1].X, (int)lineB.Points[1].Y, (int)lineB.Points[2].X, (int)lineB.Points[2].Y),
				new Segment((int)lineB.Points[2].X, (int)lineB.Points[2].Y, (int)lineB.Points[3].X, (int)lineB.Points[3].Y)
			};

			for (int i = 0; i < segmentsA.Length; i++)
			{
				for (int j = 0; j < segmentsB.Length; j++)
				{
					if (Intersect(segmentsA[i], segmentsB[j]))
					{
						intersectionsTotal++;
					}
				}
			}

			return intersectionsTotal;
		}

		private bool Intersect(Segment segmentA, Segment segmentB)
		{
			bool aIsHorizontal = segmentA.Y1 == segmentA.Y2;
			bool bIsHorizontal = segmentB.Y1 == segmentB.Y2;

			if (aIsHorizontal && !bIsHorizontal)
			{
				return IntersectsHorisontalVertical(segmentA, segmentB);
			}
			else if (bIsHorizontal && !aIsHorizontal)
			{
				return IntersectsHorisontalVertical(segmentB, segmentA);
			}

			return false;
		}

		private bool IntersectsHorisontalVertical(Segment horisontal, Segment vertical)
		{
			int ax1 = Math.Min(horisontal.X1, horisontal.X2);
			int ax2 = Math.Max(horisontal.X1, horisontal.X2);
			if (vertical.X1 > ax1 && vertical.X1 < ax2)
			{
				int by1 = Math.Min(vertical.Y1, vertical.Y2);
				int by2 = Math.Max(vertical.Y1, vertical.Y2);
				// a is horisontal, so the same y
				if (horisontal.Y1 > by1 && horisontal.Y1 < by2)
				{
					return true;
				}
			}
			return false;
		}

		#endregion Private Methods
	}
}
